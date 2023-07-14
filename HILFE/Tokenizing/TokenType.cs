using System.Diagnostics.CodeAnalysis;

namespace HILFE.Tokenizing;

public enum TokenType
{
    TypeName,
    Identifier,
    EqualsSymbol,
    LiteralString,
    LiteralNumber,
    Whitespace,
    NewLine,
    IfKeyword,
    ElseKeyword,
    ElseIfKeyword,
    WhileKeyword,
    BreakKeyword,
    ContinueKeyword,
    CodeBlockOpener,
    CodeBlockCloser,
    ExpressionOpener,
    ExpressionCloser,
    ExpressionSeparator,
}

public static class TokenTypes
{
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
            case '\n':
            case '\r':
                type = TokenType.NewLine;
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
            case ',':
                type = TokenType.ExpressionSeparator;
                return true;
        }

        type = null;
        return false;
    }
}