using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Leap.API.DB.Entities;
using Leap.API.Extensions;
using Leap.API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Leap.API.Services;

public class TokenGenerator : ITokenGenerator
{
    public const string IdClaim = "id";
    public const string UsernameClaim = "username";

    private readonly IConfiguration configuration;
    private readonly ILogger<TokenGenerator> logger;

    public TokenGenerator(IConfiguration configuration, ILogger<TokenGenerator> logger)
    {
        this.configuration = configuration;
        this.logger = logger;
    }

    /// <inheritdoc />
    public string Create(Author author)
    {
        logger.LogTrace("Creating token for author {Author}", author);

        var claims = new List<Claim>
        {
            new(IdClaim, author.Id.ToString()),
            new(UsernameClaim, author.Username),
        };

        logger.LogTrace("Adding claims {Claims}", claims);

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

        logger.LogInformation("Emitting new JWT for author {Author}", author);

        return handler.WriteToken(securityToken);
    }
}