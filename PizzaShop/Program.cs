using Microsoft.EntityFrameworkCore;
using DAL.Models;
using Services.Repositories;
using Services.Interfaces;
using Services.Service;
using Services.Utilities;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
 // Ensure this using directive is present

var builder = WebApplication.CreateBuilder(args);

var emailSettings= builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
builder.Services.AddSingleton(emailSettings);
// Add services to the container.
var conn=builder.Configuration.GetConnectionString("PizzaShopDbConnection");
builder.Services.AddDbContext<PizzashopContext>(q => q.UseNpgsql(conn));
var jwtSettings=builder.Configuration.GetSection("Jwt");
var key=Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddSingleton<JwtTokenService>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IRolesAndPermissions,RolesAndPermissions>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<RolesAndPermissionsServices>();
builder.Services.AddScoped<FileUploads>();
builder.Services.AddScoped<IJwtService, JwtTokenService>();
builder.Services.AddScoped<EncryptDecrypt>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events=new JwtBearerEvents
        {
            OnMessageReceived=context =>
            {
                 context.Token=context.Request.Cookies["jwtToken"];
                    return Task.CompletedTask;
            },
            
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew=TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
