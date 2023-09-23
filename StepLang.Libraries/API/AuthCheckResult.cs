namespace StepLang.Libraries.API;

public record AuthCheckResult(string Code, string Message)
{
    public static AuthCheckResult Success(string username) => new("success", $"Successfully authenticated as {username}.");

    public static AuthCheckResult NoIdClaim() => new("no_id_claim", "No id claim was found.");

    public static AuthCheckResult InvalidIdClaim() => new("invalid_id_claim", "Invalid id claim.");

    public static AuthCheckResult NoAuthor() => new("no_author", "No author was found.");
}