namespace Leap.Common.API;

public record CreateTokenResult(string Code, string Message, string? Token)
{
    public static CreateTokenResult Unauthorized()
        => new("unauthorized", "Invalid username or password", null);

    public static CreateTokenResult Success(string token)
        => new("success", "Token created successfully", token);
}
