using System.Diagnostics.CodeAnalysis;

namespace HILFE;

public enum TokenType
{
    TypeName,
    Identifier,
    EqualsSymbol,
    LiteralString,
    LiteralNumber,
    Whitespace,
    IfKeyword,
    ElseKeyword,
    ElseIfKeyword,
    WhileKeyword,
    BreakKeyword,
    ContinueKeyword,
    CodeBlockOpener,
    CodeBlockCloser,
    FunctionCall,
    ExpressionOpener,
    ExpressionCloser,
    StringConcatenationWhitespace,
    FunctionCallArgumentListOpener,
    EqualityOperator
}

public static class TokenTypes
{
    public static readonly TokenType[] Keywords =
    {
        TokenType.IfKeyword,
        TokenType.ElseKeyword,
        TokenType.ElseIfKeyword,
        TokenType.WhileKeyword,
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

    public static readonly TokenType[] Callables =
    {
        TokenType.FunctionCall,
    };

    public static readonly TokenType[] LineStarters =
        Keywords
            .Concat(Types)
            .Concat(Callables)
            .Concat(Whitespace)
            .ToArray();

    public static readonly TokenType[] Values =
        Literals
            .Concat(Whitespace)
            .ToArray();

    public static bool IsKnownTypeName(this string name)
    {
        return name is "string" or "double" or "int" or "bool";
    }

    public static bool TryParseKeyword(this string name, [NotNullWhen(true)] out TokenType? type)
    {
        switch (name)
        {
            case "if":
                type = TokenType.IfKeyword;
                return true;
            case "else":
                type = TokenType.ElseKeyword;
                return true;
            case "elseif":
                type = TokenType.ElseIfKeyword;
                return true;
            case "while":
                type = TokenType.WhileKeyword;
                return true;
            case "break":
                type = TokenType.BreakKeyword;
                return true;
            case "continue":
                type = TokenType.ContinueKeyword;
                return true;
            case "==":
                type = TokenType.EqualityOperator;
                return true;
        }

        type = null;
        return false;
    }

    public static bool TryParseSymbol(this char symbol, [NotNullWhen(true)] out TokenType? type)
    {
        switch (symbol)
        {
            case ' ':
                type = TokenType.Whitespace;
                return true;
            case '{':
                type = TokenType.CodeBlockOpener;
                return true;
            case '}':
                type = TokenType.CodeBlockCloser;
                return true;
            case '(':
                type = TokenType.ExpressionOpener;
                return true;
            case ')':
                type = TokenType.ExpressionCloser;
                return true;
            case '=':
                type = TokenType.EqualsSymbol;
                return true;
        }

        type = null;
        return false;
    }
}