﻿using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using InventoryManager.API.Data.Models;

namespace InventoryManager.API.Core.Configuration
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection IdentityConfiguration(this IServiceCollection services, IConfiguration config)
        {

            services.AddIdentity<IdentityUser, IdentityRole>(
                    option =>
                    {
                        option.User.RequireUniqueEmail = true;
                        option.Password.RequireDigit = true;
                        option.Password.RequiredLength = 6;
                        option.Password.RequireNonAlphanumeric = true;
                        option.Password.RequireUppercase = true;
                        option.Password.RequireLowercase = true;
                    }
                ).AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = config["JWT:Audience"],
                    ValidIssuer = config["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:ServerSecret"]))
                };
            });
            return services;
        }
    }
}
