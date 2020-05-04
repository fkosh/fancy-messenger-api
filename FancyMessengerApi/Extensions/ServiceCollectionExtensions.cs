using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace FancyMessengerApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection instance, string securityKey)
        {
            instance.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    // Enable sign checks.
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(securityKey)
                    ),
                    // Enable lifetime checks.
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    // Disable unnesesary checks.
                    ValidateAudience = false,
                    ValidateIssuer = false
                };

                options.RequireHttpsMetadata = false;
            });
        }
        
        public static void AddSwaggerGen(this IServiceCollection instance)
        {
            var appName = Assembly.GetExecutingAssembly().GetName().Name;
            
            var xmlDocsFilePath = Path.Combine(AppContext.BaseDirectory, $"{appName}.xml");
            
            if (!File.Exists(xmlDocsFilePath)) throw new Exception(
                "Swagger generation fail. XML-docs file not exists."
            );

            instance.AddSwaggerGen(options => {
                options.SwaggerDoc(
                    "v1", new OpenApiInfo { Version = "v1", Title = appName }
                );

                options.AddSecurityDefinition(
                    "bearer",
                    new OpenApiSecurityScheme {
                        Description = "JWT Authorization header. Example: \"Authorization: Bearer {value}\"",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    }
                );

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement {
                        {
                            new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "bearer"
                            },
                            Scheme = "bearer",
                            Name = "Authorization",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
                
                options.IncludeXmlComments(xmlDocsFilePath, true);
            });
        }
    }
}