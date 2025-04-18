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
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Text.Json;
using Azure;
using Microsoft.AspNetCore.Http; // Ensure this using directive is present

var builder = WebApplication.CreateBuilder(args);

var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
builder.Services.AddSingleton(emailSettings);
// Add services to the container.
var conn = builder.Configuration.GetConnectionString("PizzaShopDbConnection");
builder.Services.AddDbContext<PizzashopContext>(q => q.UseNpgsql(conn));
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddControllersWithViews();




builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRolesAndPermissions, RolesAndPermissions>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<RolesAndPermissionsServices>();
builder.Services.AddScoped<FileUploads>();
builder.Services.AddScoped<IJwtService, JwtTokenService>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<ITableAndSectionRepository, TableAndSectionRepository>();
builder.Services.AddScoped<TableAndSectionService>();
builder.Services.AddScoped<ITaxAndFeesRepository, TaxAndFeesRepository>();
builder.Services.AddScoped<TaxAndFeesService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<IOrderAppTablesRepository, OrderAppTablesRepository>();
builder.Services.AddScoped<OrderAppTablesService>();
builder.Services.AddScoped<IKOTRepository, KOTRepository>();
builder.Services.AddScoped<KOTService>();
builder.Services.AddScoped<IWaitingTokenRepository, WaitingTokenRepository>();
builder.Services.AddScoped<WaitingTokenService>();
builder.Services.AddScoped<IOrderAppMenuRepository, OrderAppMenuRepository>();
builder.Services.AddScoped<OrderAppMenuService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();




builder.Services.AddScoped<PermissionService>();

builder.Services.AddScoped<EncryptDecrypt>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["jwtToken"];
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = async context =>
            {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    var httpContext = context.HttpContext;
                    var refreshToken = httpContext.Request.Cookies["refreshToken"];
                    if (!string.IsNullOrEmpty(refreshToken))
                    {
                        var newTokenResponse = await RefreshAccessToken(refreshToken);
                        if (newTokenResponse != null)
                        {
                            httpContext.Response.Cookies.Append("jwtToken", newTokenResponse.jwtToken, new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                Expires = DateTime.UtcNow.AddDays(30)
                            });
                            httpContext.Response.Redirect(context.Request.Headers["Referer"]);
                            return;
                        }
                    }
                }
                context.Response.Redirect("/Home/Index");
                return;
            },
            OnChallenge = context =>
            {
                if (context.Request.Headers["Referer"].ToString().Contains("firstTime"))
                {
                    context.HandleResponse();
                    context.Response.Redirect(context.Request.Headers["Referer"]);
                    return Task.CompletedTask;
                }
                var token = context.Request.Cookies["jwtToken"];
                if (!context.Handled)
                {
                    context.HandleResponse();
                    if (token != null)
                    {
                        context.Response.Redirect(context.Request.Path);
                    }
                    else
                    {
                        context.Response.Redirect("/Home/Index");
                    }
                }
                return Task.CompletedTask;
            }
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
            ClockSkew = TimeSpan.Zero
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
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "OrderApp",
    pattern: "OrderApp{controller=Tables}/{action=Index}/{id?}"
);

app.Run();
async Task<TokenResponse?> RefreshAccessToken(string refresh_token)
{
    using var client = new HttpClient();
    var requestContent = new StringContent($"\"{refresh_token}\"", Encoding.UTF8, "application/json");
    var response = await client.PostAsync("http://localhost:5196/Home/RefreshToken/refresh_token", requestContent);
    if (!response.IsSuccessStatusCode) return null;
    var jsonResponse = await response.Content.ReadAsStringAsync();
    return JsonSerializer.Deserialize<TokenResponse>(jsonResponse);
}