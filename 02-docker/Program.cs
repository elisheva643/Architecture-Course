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
// 🔥 DbContext + SQL
// =======================
builder.Services.AddDbContext<ChineseOrderContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // הכתובת של אנגולר
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
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
// 🔹 AutoMapper
// =======================
builder.Services.AddAutoMapper(typeof(Program).Assembly);
// =======================
// 🔹 DI - DAL / BLL
// =======================
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

// =======================
// 🔹 Controllers + Swagger
// =======================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =======================
// 🔹 Middleware
// =======================
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowAngular");



// =======================
// 🔹 Pipeline
// =======================
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