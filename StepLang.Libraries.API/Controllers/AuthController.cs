using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StepLang.Libraries.API.DB;
using StepLang.Libraries.API.DB.Entities;
using StepLang.Libraries.API.Interfaces;
using StepLang.Libraries.API.Services;

namespace StepLang.Libraries.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly LibraryApiContext context;
    private readonly IPasswordHasher<Author> passwordHasher;
    private readonly ITokenGenerator tokenGenerator;

    public AuthController(LibraryApiContext context, ITokenGenerator tokenGenerator, IPasswordHasher<Author> passwordHasher)
    {
        this.context = context;
        this.tokenGenerator = tokenGenerator;
        this.passwordHasher = passwordHasher;
    }

    [HttpPost]
    [ProducesResponseType(typeof(RegisterResult), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(RegisterResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var existingAuthor = await context.Authors.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (existingAuthor is not null)
            return Conflict(RegisterResult.UsernameExists());

        var author = new Author
        {
            Username = request.Username,
        };

        var passwordHash = passwordHasher.HashPassword(author, request.Password);
        author.PasswordHash = passwordHash;

        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();

        var token = tokenGenerator.Create(author);

        return Ok(RegisterResult.Success(token));
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateTokenResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(CreateTokenResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Token([FromBody] CreateTokenRequest request)
    {
        var author = await context.Authors.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (author is null)
            return Unauthorized(CreateTokenResult.Unauthorized());

        var result = passwordHasher.VerifyHashedPassword(author, author.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized(CreateTokenResult.Unauthorized());

        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            author.PasswordHash = passwordHasher.HashPassword(author, request.Password);
            await context.SaveChangesAsync();
        }

        var token = tokenGenerator.Create(author);

        return Ok(CreateTokenResult.Success(token));
    }

    [HttpGet]
    [ProducesResponseType(typeof(AuthCheckResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(AuthCheckResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(AuthCheckResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(AuthCheckResult), StatusCodes.Status200OK)]
    public IActionResult Check()
    {
        var idStr = User.FindFirstValue(TokenGenerator.IdClaim);
        if (idStr is null)
            return Unauthorized(AuthCheckResult.NoIdClaim());

        if (!Guid.TryParse(idStr, out var id))
            return BadRequest(AuthCheckResult.InvalidIdClaim());

        var author = context.Authors.Find(id);
        if (author is null)
            return StatusCode(StatusCodes.Status403Forbidden, AuthCheckResult.NoAuthor());

        return Ok(AuthCheckResult.Success(author.Username));
    }
}
