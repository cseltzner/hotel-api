using API.Context;
using API.Models.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions.ServiceExtensions;

public static class IdentityServiceExtensions
{
    /// <summary>
    /// Adds configured Identity Core services
    /// </summary>
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireUppercase = true;
        }).AddEntityFrameworkStores<HotelContext>();

        services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                    options.DefaultForbidScheme =
                        options.DefaultScheme =
                            options.DefaultSignInScheme =
                                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = config["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = config["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(config["JWT:SigningKey"])
                )
            };
        });

        return services;
    }
}