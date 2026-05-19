using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectManagement.Application.Helpers;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Data;
using Microsoft.OpenApi.Models;
using System.Text;

namespace ProjectManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Database 
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            #endregion

            #region Identity 
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            #endregion


            #region JWT
            builder.Services.Configure<JWT>(
                builder.Configuration.GetSection("JWT"));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;

                options.SaveToken = true;

                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,

                        ValidIssuer =
                            builder.Configuration["JWT:Issuer"],

                        ValidAudience =
                            builder.Configuration["JWT:Audience"],

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(
                                    builder.Configuration["JWT:Key"]!)),

                        ClockSkew = TimeSpan.Zero
                    };
            });
            #endregion



            builder.Services.AddControllers();



            #region Swagger 
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Project Management API",
                    Description = "ASP.NET Core Web API"
                });

                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Enter: Bearer {token}"
                    });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
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
                            Array.Empty<string>()
                        }
                    });
            });
            #endregion

            var app = builder.Build();

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
        }
    }
}