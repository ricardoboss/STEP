namespace StepLang.Libraries.Client;

public record Credentials(string Token, string BaseAddress)
{
    public static Credentials TokenOnly(string token) => new(token, LibApiClientFactory.DefaultApiBaseAddress);
}
