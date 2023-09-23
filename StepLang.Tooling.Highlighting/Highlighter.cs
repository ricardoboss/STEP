using StepLang.Tokenizing;

namespace StepLang.Tooling.Highlighting;

public class Highlighter
{
    public IEnumerable<IEnumerable<StyledToken>> Highlight(string sourceCode, ColorScheme scheme)
    {
        var lines = sourceCode.ReplaceLineEndings().Split(Environment.NewLine);

        foreach (var line in lines)
        {
            yield return HighlightLine(line, scheme);
        }
    }

    public IEnumerable<StyledToken> HighlightLine(string lineCode, ColorScheme scheme)
    {
        var tokenizer = new Tokenizer(false);
        tokenizer.Add(lineCode);
        foreach (var token in tokenizer.Tokenize())
        {
            yield return new(token.Type, token.Value, GetStyle(token.Type, scheme));
        }
    }

    private static Style GetStyle(TokenType tokenType, ColorScheme scheme) => tokenType switch
    {
        TokenType.Identifier => scheme.Identifier,
        TokenType.TypeName => scheme.Type,
        TokenType.EqualsSymbol => scheme.Operator,
        TokenType.LiteralString => scheme.String,
        TokenType.LiteralNumber => scheme.Number,
        TokenType.LiteralBoolean => scheme.Bool,
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
        _ => scheme.Default,
    };
}