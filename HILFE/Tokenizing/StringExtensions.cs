namespace HILFE.Tokenizing;

public static class StringExtensions
{
    public static bool IsValidIdentifier(this string value)
    {
        return value.Trim().Length > 0 && !value.Contains('.') && (value[0] >= 'a' && value[0] <= 'z' || value[0] >= 'A' && value[0] <= 'Z' || value[0] == '_');
    }
}