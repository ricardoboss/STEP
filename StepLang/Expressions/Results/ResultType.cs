namespace StepLang.Expressions.Results;

public enum ResultType
{
    Void,
    Str, // cannot use String because it is a reserved keyword
    Number,
    Bool,
    List,
    Map,
    Function,
    Null,
}

public static class ValueTypeExtensions
{
    public static string ToTypeName(this ResultType resultType)
    {
        return resultType switch
        {
            ResultType.Void => "void",
            ResultType.Str => "string",
            ResultType.Number => "number",
            ResultType.Bool => "bool",
            ResultType.List => "list",
            ResultType.Map => "map",
            ResultType.Function => "function",
            ResultType.Null => "null",
            _ => throw new ArgumentOutOfRangeException(nameof(resultType), resultType, null),
        };
    }

    public static ResultType FromTypeName(string typeName)
    {
        return typeName.ToUpperInvariant() switch
        {
            "VOID" => ResultType.Void,
            "STRING" => ResultType.Str,
            "NUMBER" => ResultType.Number,
            "BOOL" => ResultType.Bool,
            "LIST" => ResultType.List,
            "MAP" => ResultType.Map,
            "FUNCTION" => ResultType.Function,
            "NULL" => ResultType.Null,
            _ => throw new ArgumentOutOfRangeException(nameof(typeName), typeName, null),
        };
    }
}