using Copilot.Infrastructure;
using Copilot.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

namespace Copilot.Api.Extentions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection collection)
    {
        collection.AddSwaggerGen(options =>
        {
            // This code adds JWT Authentication to Swagger
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcd')", 
                Name = "Authorization",
                In = ParameterLocation.Header, 
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"  // must be lower case
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    Array.Empty<string>() }
            });
        });

        return collection;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection collection)
    {
        collection.AddIdentityApiEndpoints<User>(options =>
            {
                options.Password = new PasswordOptions
                {
                    RequiredLength = 1,
                    RequiredUniqueChars = 0,
                    RequireNonAlphanumeric = false,
                    RequireLowercase = false,
                    RequireUppercase = false,
                    RequireDigit = false,
                };
                options.SignIn = new SignInOptions
                {
                    RequireConfirmedEmail = false,
                    RequireConfirmedPhoneNumber = false,
                    RequireConfirmedAccount = false
                };
            })
            .AddEntityFrameworkStores<ApplicationContext>();

        return collection;
    }
}