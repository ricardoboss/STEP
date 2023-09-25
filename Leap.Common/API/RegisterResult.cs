namespace Leap.Common.API;

public record RegisterResult(string Code, string Message, string? Token)
{
    public static RegisterResult UsernameExists()
        => new("username_exists", "Username already exists", null);

    public static RegisterResult Success(string token)
        => new("success", "User created successfully", token);
}
