using StepLang.Tokenizing;

namespace StepLang.Tooling.Highlighting;

/// <summary>
/// Highlights source code using a <see cref="ColorScheme"/>.
/// </summary>
public class Highlighter
{
    private readonly ColorScheme scheme;

    /// <summary>
    /// Creates a new <see cref="Highlighter"/> with the given <see cref="ColorScheme"/>.
    /// </summary>
    /// <param name="scheme"></param>
    public Highlighter(ColorScheme scheme)
    {
        this.scheme = scheme;
    }

    /// <summary>
    /// Highlights the given source code.
    /// </summary>
    /// <param name="sourceCode">The source code to highlight.</param>
    /// <returns>A sequence of <see cref="StyledToken"/>s.</returns>
    public IEnumerable<StyledToken> Highlight(string sourceCode)
    {
        var tokenizer = new Tokenizer(sourceCode, false);

        foreach (var token in tokenizer.Tokenize())
            yield return new(token.Type, token.Value, GetStyle(token.Type, scheme));
    }

    public static Style GetStyle(TokenType tokenType, ColorScheme scheme) => tokenType switch
    {
        TokenType.Identifier => scheme.Identifier,
        TokenType.TypeName => scheme.Type,
        TokenType.EqualsSymbol => scheme.Operator,
        TokenType.LiteralString => scheme.String,
        TokenType.LiteralNumber => scheme.Number,
        TokenType.LiteralBoolean => scheme.Bool,
        TokenType.LiteralNull => scheme.Null,
        TokenType.IfKeyword => scheme.Keyword,
        TokenType.ElseKeyword => scheme.Keyword,
        TokenType.WhileKeyword => scheme.Keyword,
        TokenType.BreakKeyword => scheme.Keyword,
        TokenType.ContinueKeyword => scheme.Keyword,
        TokenType.OpeningCurlyBracket => scheme.Punctuation,
        TokenType.ClosingCurlyBracket => scheme.Punctuation,
        TokenType.OpeningParentheses => scheme.Punctuation,
        TokenType.ClosingParentheses => scheme.Punctuation,
        TokenType.CommaSymbol => scheme.Punctuation,
        TokenType.GreaterThanSymbol => scheme.Operator,
        TokenType.LessThanSymbol => scheme.Operator,
        TokenType.PlusSymbol => scheme.Operator,
        TokenType.MinusSymbol => scheme.Operator,
        TokenType.AsteriskSymbol => scheme.Operator,
        TokenType.SlashSymbol => scheme.Operator,
        TokenType.PercentSymbol => scheme.Operator,
        TokenType.PipeSymbol => scheme.Operator,
        TokenType.AmpersandSymbol => scheme.Operator,
        TokenType.ExclamationMarkSymbol => scheme.Operator,
        TokenType.HatSymbol => scheme.Operator,
        TokenType.QuestionMarkSymbol => scheme.Operator,
        TokenType.ReturnKeyword => scheme.Keyword,
        TokenType.UnderscoreSymbol => scheme.Operator,
        TokenType.LineComment => scheme.Comment,
        TokenType.OpeningSquareBracket => scheme.Punctuation,
        TokenType.ClosingSquareBracket => scheme.Punctuation,
        TokenType.ColonSymbol => scheme.Punctuation,
        TokenType.ImportKeyword => scheme.Keyword,
        TokenType.ForEachKeyword => scheme.Keyword,
        TokenType.InKeyword => scheme.Keyword,
        _ => scheme.Default,
    };
}