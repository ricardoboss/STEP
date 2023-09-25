namespace Leap.Client;

public record Credentials(string Token, string BaseAddress)
{
    public const string DefaultApiBaseAddress = "https://lib.step-lang.dev/api/";

    public static Credentials TokenOnly(string token) => new(token, DefaultApiBaseAddress);
}
