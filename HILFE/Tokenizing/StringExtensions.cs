namespace HILFE.Tokenizing;

public static class StringExtensions
{
    public static bool IsValidIdentifier(this string value)
    {
        return !value.Contains('.');
    }
}