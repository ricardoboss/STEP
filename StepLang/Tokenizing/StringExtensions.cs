namespace StepLang.Tokenizing;

public static class StringExtensions
{
    public static bool IsValidIdentifier(this string value)
    {
        return value.Trim().Length != 0 && !value.GetFirstInvalidIdentifierCharacter().HasValue;
    }

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