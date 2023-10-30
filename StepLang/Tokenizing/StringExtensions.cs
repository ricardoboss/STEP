namespace StepLang.Tokenizing;

/// <summary>
/// Provides extension methods for the <see cref="string"/> class to help with tokenizing.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Determines whether the given string is a valid identifier.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns><c>true</c> if the string is a valid identifier; otherwise, <c>false</c>.</returns>
    public static bool IsValidIdentifier(this string value)
    {
        return value.Trim().Length != 0 && !value.GetFirstInvalidIdentifierCharacter().HasValue;
    }

    /// <summary>
    /// Gets the first invalid identifier character in the given string, or <c>null</c> if the string is a valid.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>The first invalid identifier character in the given string, or <c>null</c> if the string is a valid.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the given string is empty.</exception>
    public static char? GetFirstInvalidIdentifierCharacter(this string value)
    {
        if (value.Trim().Length == 0)
            throw new InvalidOperationException("Cannot get the first invalid identifier character of an empty string.");

        if (value.Contains('.', StringComparison.InvariantCulture))
            return '.';

        var firstChar = value[0];
        if (!char.IsAsciiLetter(firstChar) && firstChar != '_')
            return firstChar;

        var invalidChar = value[1..].FirstOrDefault(c => !char.IsAsciiLetterOrDigit(c) && c != '_');

        return invalidChar == default ? null : invalidChar;
    }
}