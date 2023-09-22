using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace StepLang.Libraries.API.Extensions;

public static class IConfigurationExtensions
{
    public static string GetJwtIssuer(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("Jwt:Issuer") ?? throw new("Jwt:Issuer is not set");
    }

    public static string GetJwtAudience(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("Jwt:Audience") ?? throw new("Jwt:Audience is not set");
    }

    public static SymmetricSecurityKey GetJwtSecretKey(this IConfiguration configuration)
    {
        var key = configuration.GetValue<string>("Jwt:Secret") ?? throw new("Jwt:Secret is not set");

        return new(Encoding.UTF8.GetBytes(key));
    }
}