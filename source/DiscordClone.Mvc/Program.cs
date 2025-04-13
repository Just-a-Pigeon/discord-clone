using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Refit;
using signalRChatMVC.Hubs;
using signalRChatMVC.Services;
using signalRChatMVC.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddSession();
builder.Services.AddSignalR();


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["JWTConfiguration:Issuer"],
            ValidAudience = config["JWTConfiguration:Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTConfiguration:SigningKey"]))
        };
    });

builder.Services.AddRefitClient<DiscordClone.Business.ApiServices.Api.IApiService>().ConfigureHttpClient(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["App:DiscordCloneApiUrl"]!);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    
    );
app.MapControllerRoute(
    name: "friends",
    pattern: "{controller=Accounts}/{action=Index}/{searchTerm?}"
    
    );
app.MapControllerRoute(
    name: "chat",
    pattern: "{controller=Chat}/{action=Chat}/{id?}"
);
app.MapControllerRoute(
    name: "chatroom",
    pattern: "{controller=Chat}/{action=ChatRooms}/{id}"
);

app.Run();