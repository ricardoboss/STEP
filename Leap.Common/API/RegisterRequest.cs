namespace Leap.Common.API;

public record RegisterRequest(string Username, string Password)
{
    public void Validate()
    {
        if (!Library.UsernameRegex().IsMatch(Username))
            throw new ValidationException("User name must only contain lowercase characters and hyphens (-) and must start and end with a character.");

        // TODO validate password strength
    }
}
