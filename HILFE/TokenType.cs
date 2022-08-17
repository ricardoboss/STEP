using System.Diagnostics.CodeAnalysis;

namespace HILFE;

public enum TokenType
{
    TypeName,
    Identifier,
    AssignmentOperator,
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

    public static bool IsBuiltInType(this string name)
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
        }

        type = null;
        return false;
    }

    public static bool TryParseSymbol(this string symbol, TokenType? previous, [NotNullWhen(true)] out TokenType? type, out TokenType[]? allowedFollowedTypes)
    {
        switch (symbol)
        {
            case " " when previous is TokenType.LiteralString:
                type = TokenType.StringConcatenationWhitespace;
                allowedFollowedTypes = new[] { TokenType.LiteralString };
                return true;
            case " ":
                type = TokenType.Whitespace;
                allowedFollowedTypes = null;
                return true;
            case "{":
                type = TokenType.CodeBlockOpener;
                allowedFollowedTypes = LineStarters;
                return true;
            case "}":
                type = TokenType.CodeBlockCloser;
                allowedFollowedTypes = LineStarters;
                return true;
            case "(" when previous is TokenType.Identifier:
                type = TokenType.FunctionCallArgumentListOpener;
                allowedFollowedTypes = Literals.Concat(new [] { TokenType.ExpressionOpener, TokenType.Identifier }).ToArray();
                return true;
            case "(":
                type = TokenType.ExpressionOpener;
                allowedFollowedTypes = Literals.Concat(new [] { TokenType.ExpressionOpener, TokenType.Identifier }).ToArray();
                return true;
            case ")":
                type = TokenType.ExpressionCloser;
                allowedFollowedTypes = new[] { TokenType.ExpressionCloser, TokenType.CodeBlockOpener };
                return true;
            case "=":
                type = TokenType.AssignmentOperator;
                allowedFollowedTypes = new[] { TokenType.LiteralString, TokenType.LiteralNumber, TokenType.ExpressionOpener, TokenType.Identifier };
                return true;
            case "==":
                type = TokenType.EqualityOperator;
                allowedFollowedTypes = new[] { TokenType.LiteralString, TokenType.LiteralNumber, TokenType.ExpressionOpener, TokenType.Identifier };
                return true;
        }

        allowedFollowedTypes = null;
        type = null;
        return false;
    }

    public static bool IsTokenSeparator(this char value)
    {
        return value is ' ';
    }
}