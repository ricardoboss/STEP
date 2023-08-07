using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

public enum TokenType
{
    TypeName,
    Identifier,
    EqualsSymbol,
    LiteralString,
    LiteralNumber,
    LiteralBoolean,
    Whitespace,
    NewLine,
    IfKeyword,
    ElseKeyword,
    WhileKeyword,
    BreakKeyword,
    ContinueKeyword,
    OpeningCurlyBracket,
    ClosingCurlyBracket,
    OpeningParentheses,
    ClosingParentheses,
    CommaSymbol,
    GreaterThanSymbol,
    LessThanSymbol,
    PlusSymbol,
    MinusSymbol,
    AsteriskSymbol,
    SlashSymbol,
    PercentSymbol,
    PipeSymbol,
    AmpersandSymbol,
    ExclamationMarkSymbol,
    HatSymbol,
    TildeSymbol,
    QuestionMarkSymbol,
    ReturnKeyword,
    UnderscoreSymbol,
    LineComment,
    OpeningSquareBracket,
    ClosingSquareBracket,
    ColonSymbol,
}

public static class TokenTypes
{
    public static bool IsKnownTypeName(this string name)
    {
        return name is "string" or "number" or "bool" or "function" or "list" or "map";
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
            case "while":
                type = TokenType.WhileKeyword;
                return true;
            case "break":
                type = TokenType.BreakKeyword;
                return true;
            case "continue":
                type = TokenType.ContinueKeyword;
                return true;
            case "return":
                type = TokenType.ReturnKeyword;
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
            case '\t':
                type = TokenType.Whitespace;
                return true;
            case '\n':
            case '\r':
                type = TokenType.NewLine;
                return true;
            case '{':
                type = TokenType.OpeningCurlyBracket;
                return true;
            case '}':
                type = TokenType.ClosingCurlyBracket;
                return true;
            case '(':
                type = TokenType.OpeningParentheses;
                return true;
            case ')':
                type = TokenType.ClosingParentheses;
                return true;
            case '[':
                type = TokenType.OpeningSquareBracket;
                return true;
            case ']':
                type = TokenType.ClosingSquareBracket;
                return true;
            case '=':
                type = TokenType.EqualsSymbol;
                return true;
            case '|':
                type = TokenType.PipeSymbol;
                return true;
            case '&':
                type = TokenType.AmpersandSymbol;
                return true;
            case '!':
                type = TokenType.ExclamationMarkSymbol;
                return true;
            case '?':
                type = TokenType.QuestionMarkSymbol;
                return true;
            case '^':
                type = TokenType.HatSymbol;
                return true;
            case '~':
                type = TokenType.TildeSymbol;
                return true;
            case '>':
                type = TokenType.GreaterThanSymbol;
                return true;
            case '<':
                type = TokenType.LessThanSymbol;
                return true;
            case '+':
                type = TokenType.PlusSymbol;
                return true;
            case '-':
                type = TokenType.MinusSymbol;
                return true;
            case '*':
                type = TokenType.AsteriskSymbol;
                return true;
            case '/':
                type = TokenType.SlashSymbol;
                return true;
            case '%':
                type = TokenType.PercentSymbol;
                return true;
            case ',':
                type = TokenType.CommaSymbol;
                return true;
            case '_':
                type = TokenType.UnderscoreSymbol;
                return true;
            case ':':
                type = TokenType.ColonSymbol;
                return true;
        }

        type = null;
        return false;
    }

    public static bool IsMathematicalOperation(this TokenType type)
    {
        return type switch
        {
            TokenType.PlusSymbol or TokenType.MinusSymbol or TokenType.AsteriskSymbol or TokenType.SlashSymbol or TokenType.PercentSymbol => true,
            _ => false,
        };
    }

    public static bool IsLiteral(this TokenType type)
    {
        return type switch
        {
            TokenType.LiteralString or TokenType.LiteralNumber or TokenType.LiteralBoolean => true,
            _ => false,
        };
    }
}