using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DiscordClone.Api.Api.Binders;
using DiscordClone.Api.Configuration;
using DiscordClone.Api.Utils;
using DiscordClone.Domain.Entities.Consultation;
using DiscordClone.Persistence;
using FastEndpoints;
using FastEndpoints.Swagger;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using OpenApiServer = NSwag.OpenApiServer;

var builder = WebApplication.CreateBuilder(args);

var config = new Configuration();
var configuration = builder.Configuration;

var datasource = DataSourceBuilder.Build(configuration);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration).WriteTo.OpenTelemetry()
    .CreateLogger();

builder.Configuration.Bind(config);
builder.Services.AddSerilog();
builder.Services.AddCors();
builder.Services.AddSingleton(typeof(IRequestBinder<>), typeof(UserIdBinder<>));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<DiscordCloneContext>();

builder.Services.AddDbContext<DiscordCloneContext>(options =>
{
    options.UseNpgsql(datasource);
    options.UseSnakeCaseNamingConvention();

    if (builder.Environment.IsDevelopment())
    {
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
});

builder.Services.AddFastEndpoints();
builder.Services.AddTransient<UserIdRetriever>();

builder.Services
    .SwaggerDocument(o =>
    {
        o.ExcludeNonFastEndpoints = true;
        o.EnableJWTBearerAuth = false;
        o.ShortSchemaNames = true;
        o.MaxEndpointVersion = 1;
        o.DocumentSettings = s =>
        {
            s.DocumentName = "Discord Clone API V1";
            s.Title = "Discord Clone API";
            s.Version = "v1";
        };
    });

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

app
    .UseFastEndpoints(c =>
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