namespace StepLang.Parsing.Expressions;

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
        return typeName switch
        {
            "void" => ResultType.Void,
            "string" => ResultType.Str,
            "number" => ResultType.Number,
            "bool" => ResultType.Bool,
            "list" => ResultType.List,
            "map" => ResultType.Map,
            "function" => ResultType.Function,
            "null" => ResultType.Null,
            _ => throw new ArgumentOutOfRangeException(nameof(typeName), typeName, null),
        };
    }
}