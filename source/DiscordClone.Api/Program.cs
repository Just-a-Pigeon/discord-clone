using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DiscordClone.Api.Api.Binders;
using DiscordClone.Api.Configuration;
using DiscordClone.Api.ServiceBus.Consumers;
using DiscordClone.Api.Utils;
using DiscordClone.Domain.Entities.Consultation;
using DiscordClone.Persistence;
using FastEndpoints;
using FastEndpoints.OpenTelemetry;
using FastEndpoints.Swagger;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Sinks.OpenTelemetry;
using OpenApiServer = NSwag.OpenApiServer;

var builder = WebApplication.CreateBuilder(args);

var config = new Configuration();
var configuration = builder.Configuration;
builder.Configuration.Bind(config);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.OpenTelemetry(options =>
    {
        options.Protocol = OtlpProtocol.HttpProtobuf;
    })
    .CreateLogger();

builder.Services.AddSerilog();
builder.Services.AddCors();
builder.Services.AddFastEndpoints();
builder.Services.AddSingleton(typeof(IRequestBinder<>), typeof(UserIdBinder<>));
builder.Services.AddTransient<UserIdRetriever>();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService("Discord Clone API")
        .AddAttributes(new List<KeyValuePair<string, object>>
        {
            new("Startup", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
            new("AppVersion", "v1"),
            new("deployment.environment", "development")
        }))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation(options =>
        {
            options.Filter = context => 
                !context.Request.Path.StartsWithSegments("/hangfire") && 
                !context.Request.Path.StartsWithSegments("/health");
        })
        .AddEntityFrameworkCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddFastEndpointsInstrumentation()
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(builder.Configuration["Otlp"]!);
            options.Protocol = OtlpExportProtocol.HttpProtobuf;
        }));
;

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<DiscordCloneContext>();

builder.Services.AddDbContext<DiscordCloneContext>(options =>
{
    var datasource = DataSourceBuilder.Build(configuration);
    options.UseNpgsql(datasource);
    options.UseSnakeCaseNamingConvention();

    if (builder.Environment.IsDevelopment())
    {
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
});

builder.Services.SwaggerDocument(o =>
{
    o.ExcludeNonFastEndpoints = true;
    o.EnableJWTBearerAuth = true;
    o.ShortSchemaNames = true;
    o.MaxEndpointVersion = 1;
    o.DocumentSettings = s =>
    {
        s.DocumentName = "Discord Clone API V1";
        s.Title = "Discord Clone API";
        s.Version = "v1";
    };
});
builder.Services.AddSwaggerGen(o => o.OperationFilter<AddCommonResponses>());

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtOptions =>
{
    var builderConfiguration = builder.Configuration;
    jwtOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builderConfiguration["JWTConfiguration:Issuer"],
        ValidAudience = builderConfiguration["JWTConfiguration:Audience"],
        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builderConfiguration["JWTConfiguration:SigningKey"] ?? string.Empty))
    };
});

builder.Services.AddAuthorizationBuilder();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<DeleteMessagesOfDeletedUserConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("discord-clone-api"));
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.UseJsonSerializer();
        cfg.UseJsonDeserializer();
        cfg.ConfigureJsonSerializerOptions(o =>
        {
            o.PropertyNameCaseInsensitive = true;
            return o;
        });
        
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    EntityFrameworkProfiler.Initialize();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
    app.UseForwardedHeaders();
}

app.UseFastEndpoints(c =>
    {
        c.Endpoints.RoutePrefix = "api";
        c.Versioning.Prefix = "v";
        c.Versioning.PrependToRoute = true;
        c.Versioning.DefaultVersion = 1;
        c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        c.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
    })
    .UseSwaggerGen(s =>
    {
        s.PostProcess = (o, h) =>
        {
            o.Host = config.SwaggerServerHost;
            o.Servers.Clear();
            o.Servers.Add(new OpenApiServer
            {
                Url = config.SwaggerServerUrl,
                Description = "Discord Clone API"
            });
        };
    });

app.Run();