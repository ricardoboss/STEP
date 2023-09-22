using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using StepLang.Libraries.API.DB.Entities;
using StepLang.Libraries.API.Extensions;
using StepLang.Libraries.API.Interfaces;

namespace StepLang.Libraries.API.Services;

public class TokenGenerator : ITokenGenerator
{
    public const string IdClaim = "id";
    public const string UsernameClaim = "username";

    private readonly IConfiguration configuration;

    public TokenGenerator(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    /// <inheritdoc />
    public string Create(Author owner)
    {
        var claims = new List<Claim>
        {
            new(IdClaim, owner.Id.ToString()),
            new(UsernameClaim, owner.Username),
        };

        var key = configuration.GetJwtSecretKey();
        var issuer = configuration.GetJwtIssuer();
        var audience = configuration.GetJwtAudience();

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new(claims),
            Expires = DateTime.UtcNow.AddDays(30),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new(key, SecurityAlgorithms.HmacSha256),
        };

        var handler = new JwtSecurityTokenHandler();
        var securityToken = handler.CreateToken(descriptor);

        return handler.WriteToken(securityToken);
    }
}