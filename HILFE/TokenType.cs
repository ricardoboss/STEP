namespace HILFE;

public enum TokenType
{
    TypeName,
    Identifier,
    Keyword,
    MathOperator,
    BinaryOperator,
    UnaryOperator,
    AssignmentOperator,
    LiteralString,
    LiteralNumber,
    LineComment,
    Whitespace,
}

public static class TokenTypes
{
    public static readonly TokenType[] Keywords =
    {
        TokenType.Keyword,
    };

    public static readonly TokenType[] Literals =
    {
        TokenType.LiteralString,
        TokenType.LiteralNumber,
    };

    public static readonly TokenType[] Whitespace =
    {
        TokenType.Whitespace,
    };

    public static readonly TokenType[] Types =
    {
        TokenType.TypeName,
    };

    public static readonly TokenType[] LineStarters = Keywords.Concat(Types).Concat(Whitespace).ToArray();

    public static readonly TokenType[] Values = Literals.Concat(Whitespace).ToArray();

    public static readonly TokenType[] All = Enum.GetValues<TokenType>();

    public static bool IsBuiltInType(this string name)
    {
        return name is "string" or "double" or "int" or "bool";
    }
}