using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

/// <summary>
/// The token type determines how the token is interpreted by the parser.
/// Tokens are the building blocks of a program and their type determines their behaviour.
/// </summary>
public enum TokenType
{
    /// <summary>
    /// A type name is a string of characters that can be used for types.
    /// </summary>
    TypeName,

    /// <summary>
    /// An identifier is a string of characters that can be used for variable names.
    /// </summary>
    Identifier,

    /// <summary>
    /// The equals symbol <c>=</c>
    /// </summary>
    EqualsSymbol,

    /// <summary>
    /// A literal string is a string of characters surrounded by double quotes.
    /// </summary>
    LiteralString,

    /// <summary>
    /// A literal number is a string of characters that represent a number.
    /// </summary>
    LiteralNumber,

    /// <summary>
    /// A literal boolean is either <c>true</c> or <c>false</c>.
    /// </summary>
    LiteralBoolean,

    /// <summary>
    /// A literal <c>null</c> value.
    /// </summary>
    LiteralNull,

    /// <summary>
    /// Whitespace is a string of spaces or tabs.
    /// </summary>
    Whitespace,

    /// <summary>
    /// A new line is a string of characters that represent a new line (<c>\n</c> or <c>\r\n</c>).
    /// </summary>
    NewLine,

    /// <summary>
    /// The <c>if</c> keyword.
    /// </summary>
    IfKeyword,

    /// <summary>
    /// The <c>else</c> keyword.
    /// </summary>
    ElseKeyword,

    /// <summary>
    /// The <c>while</c> keyword.
    /// </summary>
    WhileKeyword,

    /// <summary>
    /// The <c>break</c> keyword.
    /// </summary>
    BreakKeyword,

    /// <summary>
    /// The <c>continue</c> keyword.
    /// </summary>
    ContinueKeyword,

    /// <summary>
    /// The <c>{</c> symbol.
    /// </summary>
    OpeningCurlyBracket,

    /// <summary>
    /// The <c>}</c> symbol.
    /// </summary>
    ClosingCurlyBracket,

    /// <summary>
    /// The <c>(</c> symbol.
    /// </summary>
    OpeningParentheses,

    /// <summary>
    /// The <c>)</c> symbol.
    /// </summary>
    ClosingParentheses,

    /// <summary>
    /// The <c>,</c> symbol.
    /// </summary>
    CommaSymbol,

    /// <summary>
    /// The <c>&gt;</c> symbol.
    /// </summary>
    GreaterThanSymbol,

    /// <summary>
    /// The <c>&lt;</c> symbol.
    /// </summary>
    LessThanSymbol,

    /// <summary>
    /// The <c>+</c> symbol.
    /// </summary>
    PlusSymbol,

    /// <summary>
    /// The <c>-</c> symbol.
    /// </summary>
    MinusSymbol,

    /// <summary>
    /// The <c>*</c> symbol.
    /// </summary>
    AsteriskSymbol,

    /// <summary>
    /// The <c>/</c> symbol.
    /// </summary>
    SlashSymbol,

    /// <summary>
    /// The <c>%</c> symbol.
    /// </summary>
    PercentSymbol,

    /// <summary>
    /// The <c>|</c> symbol.
    /// </summary>
    PipeSymbol,

    /// <summary>
    /// The <c>&amp;</c> symbol.
    /// </summary>
    AmpersandSymbol,

    /// <summary>
    /// The <c>!</c> symbol.
    /// </summary>
    ExclamationMarkSymbol,

    /// <summary>
    /// The <c>^</c> symbol.
    /// </summary>
    HatSymbol,

    /// <summary>
    /// The <c>?</c> symbol.
    /// </summary>
    QuestionMarkSymbol,

    /// <summary>
    /// The <c>return</c> keyword.
    /// </summary>
    ReturnKeyword,

    /// <summary>
    /// The <c>_</c> symbol.
    /// </summary>
    UnderscoreSymbol,

    /// <summary>
    /// A line comment is a string of characters that represent a comment.
    /// </summary>
    LineComment,

    /// <summary>
    /// The <c>[</c> symbol.
    /// </summary>
    OpeningSquareBracket,

    /// <summary>
    /// The <c>]</c> symbol.
    /// </summary>
    ClosingSquareBracket,

    /// <summary>
    /// The <c>:</c> symbol.
    /// </summary>
    ColonSymbol,

    /// <summary>
    /// The <c>import</c> keyword.
    /// </summary>
    ImportKeyword,

    /// <summary>
    /// The <c>foreach</c> keyword.
    /// </summary>
    ForEachKeyword,

    /// <summary>
    /// The <c>in</c> keyword.
    /// </summary>
    InKeyword,

    /// <summary>
    /// Represents the end of a file.
    /// </summary>
    EndOfFile,
}

/// <summary>
/// Extension methods for <see cref="TokenType"/>.
/// </summary>
public static class TokenTypes
{
    /// <summary>
    /// Returns a string that can be used to display the token type for development purposes or for user feedback.
    /// </summary>
    /// <param name="type">The token type.</param>
    /// <returns>A string that can be used to display the token type for development purposes or for user feedback.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the token type is unknown.</exception>
    [ExcludeFromCodeCoverage]
    public static string ToDisplay(this TokenType type)
    {
        return type switch
        {
            TokenType.TypeName => "type name",
            TokenType.Identifier => "identifier",
            TokenType.EqualsSymbol => "'='",
            TokenType.LiteralString => "literal string",
            TokenType.LiteralNumber => "literal number",
            TokenType.LiteralBoolean => "literal boolean",
            TokenType.LiteralNull => "null",
            TokenType.Whitespace => "whitespace",
            TokenType.NewLine => "new line",
            TokenType.IfKeyword => "'if'",
            TokenType.ElseKeyword => "'else'",
            TokenType.WhileKeyword => "'while'",
            TokenType.BreakKeyword => "'break'",
            TokenType.ContinueKeyword => "'continue'",
            TokenType.OpeningCurlyBracket => "'{'",
            TokenType.ClosingCurlyBracket => "'}'",
            TokenType.OpeningParentheses => "'('",
            TokenType.ClosingParentheses => "')'",
            TokenType.CommaSymbol => "','",
            TokenType.GreaterThanSymbol => "'>'",
            TokenType.LessThanSymbol => "'<'",
            TokenType.PlusSymbol => "'+'",
            TokenType.MinusSymbol => "'-'",
            TokenType.AsteriskSymbol => "'*'",
            TokenType.SlashSymbol => "'/'",
            TokenType.PercentSymbol => "'%'",
            TokenType.PipeSymbol => "'|'",
            TokenType.AmpersandSymbol => "'&'",
            TokenType.ExclamationMarkSymbol => "'!'",
            TokenType.HatSymbol => "'^'",
            TokenType.QuestionMarkSymbol => "'?'",
            TokenType.ReturnKeyword => "'return'",
            TokenType.UnderscoreSymbol => "'_'",
            TokenType.LineComment => "line comment",
            TokenType.OpeningSquareBracket => "'['",
            TokenType.ClosingSquareBracket => "']'",
            TokenType.ColonSymbol => "':'",
            TokenType.ImportKeyword => "'import'",
            TokenType.ForEachKeyword => "'foreach'",
            TokenType.InKeyword => "'in'",
            TokenType.EndOfFile => "end of file",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown token type"),
        };
    }

    /// <summary>
    /// Returns a string that represents the same token in source code.
    /// </summary>
    /// <param name="type">The token type.</param>
    /// <returns>A string that represents the same token in source code.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the token type does not have a static code representation.</exception>
    public static string ToCode(this TokenType? type)
    {
        return type switch
        {
            TokenType.BreakKeyword => "break",
            TokenType.ContinueKeyword => "continue",
            TokenType.ElseKeyword => "else",
            TokenType.IfKeyword => "if",
            TokenType.ImportKeyword => "import",
            TokenType.ReturnKeyword => "return",
            TokenType.WhileKeyword => "while",
            TokenType.AmpersandSymbol => "&",
            TokenType.AsteriskSymbol => "*",
            TokenType.ClosingCurlyBracket => "}",
            TokenType.ClosingParentheses => ")",
            TokenType.ColonSymbol => ":",
            TokenType.CommaSymbol => ",",
            TokenType.EqualsSymbol => "=",
            TokenType.ExclamationMarkSymbol => "!",
            TokenType.GreaterThanSymbol => ">",
            TokenType.HatSymbol => "^",
            TokenType.LessThanSymbol => "<",
            TokenType.MinusSymbol => "-",
            TokenType.OpeningCurlyBracket => "{",
            TokenType.OpeningParentheses => "(",
            TokenType.PercentSymbol => "%",
            TokenType.PipeSymbol => "|",
            TokenType.PlusSymbol => "+",
            TokenType.QuestionMarkSymbol => "?",
            TokenType.SlashSymbol => "/",
            TokenType.UnderscoreSymbol => "_",
            TokenType.OpeningSquareBracket => "[",
            TokenType.ClosingSquareBracket => "]",
            TokenType.ForEachKeyword => "foreach",
            TokenType.InKeyword => "in",
            TokenType.LiteralNull => "null",
            _ => throw new InvalidOperationException("Token type does not have a static code representation"),
        };
    }

    /// <summary>
    /// Checks if the given string is a known type name. Case insensitive.
    /// </summary>
    /// <param name="name">The string to check.</param>
    /// <returns><c>true</c> if the given string is a known type name; otherwise, <c>false</c>.</returns>
    public static bool IsKnownTypeName(this string name)
    {
        return name.ToUpperInvariant() is "STRING" or "NUMBER" or "BOOL" or "FUNCTION" or "LIST" or "MAP";
    }

    /// <summary>
    /// Checks if the given string is a known keyword. Case insensitive.
    /// </summary>
    /// <param name="name">The string to check.</param>
    /// <param name="type">The token type of the keyword.</param>
    /// <returns><c>true</c> if the given string is a known keyword; otherwise, <c>false</c>.</returns>
    public static bool TryParseKeyword(this string name, [NotNullWhen(true)] out TokenType? type)
    {
        switch (name.ToUpperInvariant())
        {
            case "IF":
                type = TokenType.IfKeyword;
                return true;
            case "ELSE":
                type = TokenType.ElseKeyword;
                return true;
            case "WHILE":
                type = TokenType.WhileKeyword;
                return true;
            case "BREAK":
                type = TokenType.BreakKeyword;
                return true;
            case "CONTINUE":
                type = TokenType.ContinueKeyword;
                return true;
            case "RETURN":
                type = TokenType.ReturnKeyword;
                return true;
            case "IMPORT":
                type = TokenType.ImportKeyword;
                return true;
            case "FOREACH":
                type = TokenType.ForEachKeyword;
                return true;
            case "IN":
                type = TokenType.InKeyword;
                return true;
        }

        type = null;
        return false;
    }

    /// <summary>
    /// Checks if the given character is a known symbol.
    /// </summary>
    /// <param name="symbol">The character to check.</param>
    /// <param name="type">The token type of the symbol.</param>
    /// <returns><c>true</c> if the given character is a known symbol; otherwise, <c>false</c>.</returns>
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

    public static readonly TokenType[] ShorthandMathematicalOperations = {
        TokenType.PlusSymbol,
        TokenType.MinusSymbol,
    };

    public static bool IsShorthandMathematicalOperation(this TokenType type)
    {
        return type switch
        {
            TokenType.PlusSymbol or TokenType.MinusSymbol => true,
            _ => false,
        };
    }

    public static readonly TokenType[] ShorthandMathematicalOperationsWithAssignment = {
        TokenType.PlusSymbol,
        TokenType.MinusSymbol,
        TokenType.AsteriskSymbol,
        TokenType.SlashSymbol,
        TokenType.PercentSymbol,
        TokenType.PipeSymbol,
        TokenType.AmpersandSymbol,
        TokenType.HatSymbol,
        TokenType.QuestionMarkSymbol,
    };

    public static bool IsShorthandMathematicalOperationWithAssignment(this TokenType type)
    {
        return type switch
        {
            TokenType.PlusSymbol or TokenType.MinusSymbol or TokenType.AsteriskSymbol or TokenType.SlashSymbol or TokenType.PercentSymbol or TokenType.PipeSymbol or TokenType.AmpersandSymbol or TokenType.HatSymbol or TokenType.QuestionMarkSymbol => true,
            _ => false,
        };
    }

    public static readonly TokenType[] MathematicalOperations = {
        TokenType.PlusSymbol,
        TokenType.MinusSymbol,
        TokenType.AsteriskSymbol,
        TokenType.SlashSymbol,
        TokenType.PercentSymbol,
    };

    /// <summary>
    /// Checks if the token type is a mathematical operation.
    /// </summary>
    /// <param name="type">The token type.</param>
    /// <returns><c>true</c> if the token type is a mathematical operation; otherwise, <c>false</c>.</returns>
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
            TokenType.LiteralString or TokenType.LiteralNumber or TokenType.LiteralBoolean or TokenType.LiteralNull => true,
            _ => false,
        };
    }

    public static readonly TokenType[] Operators = {
        TokenType.PlusSymbol,
        TokenType.MinusSymbol,
        TokenType.AsteriskSymbol,
        TokenType.SlashSymbol,
        TokenType.PercentSymbol,
        TokenType.PipeSymbol,
        TokenType.AmpersandSymbol,
        TokenType.ExclamationMarkSymbol,
        TokenType.HatSymbol,
        TokenType.QuestionMarkSymbol,
        TokenType.EqualsSymbol,
        TokenType.GreaterThanSymbol,
        TokenType.LessThanSymbol,
    };

    public static bool IsOperator(this TokenType type)
    {
        return type switch
        {
            TokenType.PlusSymbol or TokenType.MinusSymbol or TokenType.AsteriskSymbol or TokenType.SlashSymbol or TokenType.PercentSymbol or TokenType.PipeSymbol or TokenType.AmpersandSymbol or TokenType.ExclamationMarkSymbol or TokenType.HatSymbol or TokenType.QuestionMarkSymbol or TokenType.EqualsSymbol or TokenType.GreaterThanSymbol or TokenType.LessThanSymbol => true,
            _ => false,
        };
    }

    public static bool HasMeaning(this TokenType type)
    {
        return type switch
        {
            TokenType.Whitespace or TokenType.LineComment => false,
            _ => true,
        };
    }
}