using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using server;
using server.BLL;
using server.BLL.Interface;
using server.DAL;
using server.DAL.Interface;
using server.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using server.Middleware;

var builder = WebApplication.CreateBuilder(args);

// =======================
// 🔥 DbContext + SQL (עם מנגנון Retry)
// =======================
builder.Services.AddDbContext<ChineseOrderContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        })
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// =======================
// 🚀 Redis Cache Configuration
// =======================
var redisConnectionString = builder.Configuration.GetValue<string>("Redis__ConnectionString") ?? "redis_cache:6379";

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    options.InstanceName = "ChineseOrder_";
});

// =======================
// 🔑 JWT Authentication 
// =======================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        };
    });

// =======================
// 🔹 Dependency Injection (DI)
// =======================
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<IDonorDAL, DonorDAL>();
builder.Services.AddScoped<IDonorBLL, DonorBLL>();
builder.Services.AddScoped<IGiftDAL, GiftDAL>();
builder.Services.AddScoped<IGiftBLL, GiftBLL>();
builder.Services.AddScoped<IUserDAL, UserDAL>();
builder.Services.AddScoped<IUserBLL, UserBLL>();
builder.Services.AddScoped<IWinnerBLL, WinnerBLL>();
builder.Services.AddScoped<IWinnerDAL, WinnerDAL>();
builder.Services.AddScoped<IPurchaseBLL, PurchaseBLL>();
builder.Services.AddScoped<IPurchaseDAL, PurchaseDAL>();
builder.Services.AddScoped<IRvevnueBLL, RvevnueBLL>();
builder.Services.AddScoped<IRvevnueDAL, RvevnueDAL>();
builder.Services.AddScoped<IGiftDescriptionBLL, GiftDescriptionBLL>();
builder.Services.AddScoped<IGiftDescriptionDAL, GiftDescriptionDAL>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =======================
// 🏗️ יצירה אוטומטית של בסיס הנתונים (חשוב ל-Docker!)
// =======================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ChineseOrderContext>();
        context.Database.EnsureCreated();
        Console.WriteLine("✅ Database connected and verified.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Database connection error: " + ex.Message);
    }
}

// =======================
// 🔹 Middleware Pipeline
// =======================
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("AllowAngular");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();