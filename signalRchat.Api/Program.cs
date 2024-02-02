using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using signalRchat.Core.Entities;
using signalRchat.Core.Repositories.Interfaces;
using signalRchat.Core.Services;
using signalRchat.Core.Services.Interfaces;
using signalRchat.Infrastructure.Data;
using signalRchat.Infrastructure.Respositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors();
// Add services to the container.
builder.Services.AddSingleton<MongoMessageDbContext>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("LocalDatabase");
    options.UseSqlServer(connectionString);
});//
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IFriendshipRepository, FriendshipRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IFriendshipService, FriendshipService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",new OpenApiInfo
    {
        Title = "SignalRChat Test",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' and then your valid token.",

    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtOptions =>
{
    var config = builder.Configuration;
    jwtOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = config["JWTConfiguration:Issuer"],
        ValidAudience = config["JWTConfiguration:Audience"],
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTConfiguration:SigningKey"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanRead", policy =>
    {
        policy.RequireAssertion(context =>
        {
            if (context.User.IsInRole("Admin") || context.User.IsInRole("User"))
                return true;
            return false;
        });
    });
    options.AddPolicy("CanEdit", policy =>
    {
        policy.RequireAssertion(context =>
        {
            if (context.User.IsInRole("Admin") || context.User.IsInRole("User"))
                return true;
            return false;
        });
    });
    options.AddPolicy("CanUpdate", policy =>
    {
        policy.RequireAssertion(context =>
        {
            if (context.User.IsInRole("Admin") || context.User.IsInRole("User"))
                return true;
            return false;
        });
    });
    options.AddPolicy("CanDelete", policy =>
    {
        policy.RequireAssertion(context =>
        {
            if (context.User.IsInRole("Admin") || context.User.IsInRole("User"))
                return true;
            return false;
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
