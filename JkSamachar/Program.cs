using JKSamachar.DAL.Common;
using JKSamachar.DAL.Data;
using JKSamachar.DAL.Enitity;
using JKSamachar.DAL.Repository.IRepository;
using JKSamachar.DAL.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using JKSamachar.Services.Interfaces;
using JKSamachar.Services.Implementation;
using JKSamachar.DTO.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<JKSamacharContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<AppUser, IdentityRole>()
   .AddEntityFrameworkStores<JKSamacharContext>()
   .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});
var appSettingsSection = builder.Configuration.GetSection("AppSetting");
builder.Services.Configure<AppSetting>(appSettingsSection);

var appSettings = appSettingsSection.Get<AppSetting>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    jwt.RequireHttpsMetadata = false;
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = appSettingsSection["Issuer"],
        ValidAudience = appSettingsSection["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret))
    };
});
builder.Services.AddSingleton(appSettings);

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
builder.Services.AddControllers();
builder.Services.AddScoped<IUnitOfWork>(ctx => new UnitOfWork(ctx.GetRequiredService<JKSamacharContext>()));
builder.Services.AddTransient<IJKNewsServices, JKNewsServices>();
builder.Services.AddTransient<IAuthenticateService, AuthenticateService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.  

app.UseSwagger();
app.UseSwaggerUI();
// app.UseDeveloperExceptionPage();  

app.UseAuthentication();
app.UseAuthorization();

// Use CORS policy  
app.UseCors(x => x
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();

app.Run();