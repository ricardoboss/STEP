namespace StepLang.Expressions.Results;

/// <summary>
/// <para>
/// Represents the type of an <see cref="ExpressionResult"/>.
/// </para>
/// <para>
/// This defines the fundamental types of the STEP language.
/// </para>
/// </summary>
public enum ResultType
{
    /// <summary>
    /// Represents a result that has no value.
    /// </summary>
    /// <seealso cref="VoidResult"/>
    Void,

    /// <summary>
    /// Represents a result that has a <see cref="string"/> value.
    /// </summary>
    /// <remarks>
    /// Cannot use <c>String</c> as the enum case because it is a reserved keyword in C#.
    /// </remarks>
    /// <seealso cref="StringResult"/>
    Str,

    /// <summary>
    /// Represents a result that has a <see cref="double"/>/<see cref="int"/> value.
    /// </summary>
    /// <seealso cref="NumberResult"/>
    Number,

    /// <summary>
    /// Represents a result that has a <see cref="bool"/> value.
    /// </summary>
    /// <seealso cref="BoolResult"/>
    Bool,

    /// <summary>
    /// Represents a result that has a <see cref="List{ExpressionResult}"/> value.
    /// </summary>
    /// <seealso cref="ListResult"/>
    List,

    /// <summary>
    /// Represents a result that has a <see cref="Dictionary{String, ExpressionResult}"/> value.
    /// </summary>
    /// <seealso cref="MapResult"/>
    Map,

    /// <summary>
    /// Represents a result that has a <see cref="FunctionDefinition"/> value.
    /// </summary>
    /// <see cref="FunctionResult"/>
    Function,

    /// <summary>
    /// Represents a result that has a <see langword="null"/> value.
    /// </summary>
    /// <seealso cref="NullResult"/>
    Null,
}

/// <summary>
/// A collection of extension methods for <see cref="ResultType"/>.
/// </summary>
public static class ResultTypes
{
    /// <summary>
    /// Converts the <see cref="ResultType"/> to a <see cref="string"/> representation as it would appear in source code.
    /// </summary>
    /// <param name="resultType">The <see cref="ResultType"/> to convert.</param>
    /// <returns>The <see cref="string"/> representation of the <see cref="ResultType"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the <see cref="ResultType"/> is not recognized.</exception>
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

    /// <summary>
    /// Tries to convert the given <see cref="string"/> to a <see cref="ResultType"/>.
    /// </summary>
    /// <param name="typeName">The <see cref="string"/> to convert.</param>
    /// <returns>The <see cref="ResultType"/> represented by the <see cref="string"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the <see cref="string"/> is not recognized.</exception>
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