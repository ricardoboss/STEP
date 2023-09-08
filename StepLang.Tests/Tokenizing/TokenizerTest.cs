using StepLang.Tokenizing;

namespace StepLang.Tests.Tokenizing;

public class TokenizerTest
{
    [Fact]
    public void TestTokenizeLiteralString()
    {
        const string source = "\"abc\"";

        var tokenizer = new Tokenizer();
        tokenizer.Add(source);
        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Single(tokens);
        Assert.Equal(TokenType.LiteralString, tokens[0].Type);
        Assert.Equal("abc", tokens[0].Value);
    }

    [Fact]
    public void TestTokenizeLiteralStringWithEscapedChars()
    {
        const string source = "\"\\n\"";

        var tokenizer = new Tokenizer();
        tokenizer.Add(source);
        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Single(tokens);
        Assert.Equal(TokenType.LiteralString, tokens[0].Type);
        Assert.Equal("\n", tokens[0].Value);
    }

    [Fact]
    public void TestTokenizeLiteralNumber()
    {
        const string source = "123";

        var tokenizer = new Tokenizer();
        tokenizer.Add(source);
        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Single(tokens);
        Assert.Equal(TokenType.LiteralNumber, tokens[0].Type);
        Assert.Equal("123", tokens[0].Value);
    }

    [Fact]
    public void TestTokenizeLiteralNegativeNumber()
    {
        const string source = "-123";

        var tokenizer = new Tokenizer();
        tokenizer.Add(source);
        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal(2, tokens.Length);
        Assert.Equal(TokenType.MinusSymbol, tokens[0].Type);
        Assert.Equal("-", tokens[0].Value);
        Assert.Equal(TokenType.LiteralNumber, tokens[1].Type);
        Assert.Equal("123", tokens[1].Value);
    }

    [Fact]
    public void TestTokenizeLiteralNegativeNumberWithDecimals()
    {
        const string source = "-1.23";

        var tokenizer = new Tokenizer();
        tokenizer.Add(source);
        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal(2, tokens.Length);
        Assert.Equal(TokenType.MinusSymbol, tokens[0].Type);
        Assert.Equal("-", tokens[0].Value);
        Assert.Equal(TokenType.LiteralNumber, tokens[1].Type);
        Assert.Equal("1.23", tokens[1].Value);
    }

    [Fact]
    public void TestTokenizeLiteralStringWithSpaces()
    {
        const string source = "\"abc def\"";

        var tokenizer = new Tokenizer();
        tokenizer.Add(source);
        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Single(tokens);
        Assert.Equal(TokenType.LiteralString, tokens[0].Type);
        Assert.Equal("abc def", tokens[0].Value);
    }

    [Theory]
    [InlineData("\"abc\\\"def\"", "abc\"def")]
    public void TestTokenizeLiteralStringWithEscapedQuotes(string source, string expected)
    {
        var tokenizer = new Tokenizer();
        tokenizer.Add(source);
        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Single(tokens);
        Assert.Equal(TokenType.LiteralString, tokens[0].Type);
        Assert.Equal(expected, tokens[0].Value);
    }

    [Theory]
    [InlineData("string")]
    [InlineData("number")]
    [InlineData("bool")]
    [InlineData("function")]
    public void TestTokenizeKnownType(string source)
    {
        var tokenizer = new Tokenizer();
        tokenizer.Add(source);
        var tokens = tokenizer.Tokenize().ToList();

        Assert.Single(tokens);
        Assert.Equal(TokenType.TypeName, tokens[0].Type);
        Assert.Equal(source, tokens[0].Value);
    }

    [Theory]
    [InlineData("if", TokenType.IfKeyword)]
    [InlineData("else", TokenType.ElseKeyword)]
    [InlineData("while", TokenType.WhileKeyword)]
    [InlineData("break", TokenType.BreakKeyword)]
    [InlineData("continue", TokenType.ContinueKeyword)]
    public void TestTokenizeKeyword(string source, TokenType type)
    {
        var tokenizer = new Tokenizer();
        tokenizer.Add(source);
        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Single(tokens);
        Assert.Equal(type, tokens[0].Type);
        Assert.Equal(source, tokens[0].Value);
    }

    [Fact]
    public void TestTokenizeMultipleTokens()
    {
        const string source = "number identifier = 1";

        var tokenizer = new Tokenizer();
        tokenizer.Add(source);
        var tokens = tokenizer.Tokenize().ToList();

        Assert.Equal(7, tokens.Count);
        Assert.Equal(TokenType.TypeName, tokens[0].Type);
        Assert.Equal("number", tokens[0].Value);
        Assert.Equal(TokenType.Whitespace, tokens[1].Type);
        Assert.Equal(" ", tokens[1].Value);
        Assert.Equal(TokenType.Identifier, tokens[2].Type);
        Assert.Equal("identifier", tokens[2].Value);
        Assert.Equal(TokenType.Whitespace, tokens[3].Type);
        Assert.Equal(" ", tokens[3].Value);
        Assert.Equal(TokenType.EqualsSymbol, tokens[4].Type);
        Assert.Equal("=", tokens[4].Value);
        Assert.Equal(TokenType.Whitespace, tokens[5].Type);
        Assert.Equal(" ", tokens[5].Value);
        Assert.Equal(TokenType.LiteralNumber, tokens[6].Type);
        Assert.Equal("1", tokens[6].Value);
    }

    [Fact]
    public void TestTokenizeKeywords()
    {
        const string source = "if (true)";

        var tokenizer = new Tokenizer();
        tokenizer.Add(source);
        var tokens = tokenizer.Tokenize().ToList();

        Assert.Equal(5, tokens.Count);
        Assert.Equal(TokenType.IfKeyword, tokens[0].Type);
        Assert.Equal("if", tokens[0].Value);
        Assert.Equal(TokenType.Whitespace, tokens[1].Type);
        Assert.Equal(" ", tokens[1].Value);
        Assert.Equal(TokenType.OpeningParentheses, tokens[2].Type);
        Assert.Equal("(", tokens[2].Value);
        Assert.Equal(TokenType.LiteralBoolean, tokens[3].Type);
        Assert.Equal("true", tokens[3].Value);
        Assert.Equal(TokenType.ClosingParentheses, tokens[4].Type);
        Assert.Equal(")", tokens[4].Value);
    }

    [Fact]
    public void TestTokenizeFunctionCall()
    {
        const string source = "print(\"hello\")";

        var tokenizer = new Tokenizer();
        tokenizer.Add(source);
        var tokens = tokenizer.Tokenize().ToList();

        Assert.Equal(4, tokens.Count);
        Assert.Equal(TokenType.Identifier, tokens[0].Type);
        Assert.Equal("print", tokens[0].Value);
        Assert.Equal(TokenType.OpeningParentheses, tokens[1].Type);
        Assert.Equal("(", tokens[1].Value);
        Assert.Equal(TokenType.LiteralString, tokens[2].Type);
        Assert.Equal("hello", tokens[2].Value);
        Assert.Equal(TokenType.ClosingParentheses, tokens[3].Type);
        Assert.Equal(")", tokens[3].Value);
    }

    [Fact]
    public void TestTokenizeLineComment()
    {
        const string source = "f(variable) // this is a comment\nprintln(\"text\") // more comments";

        var tokenizer = new Tokenizer();
        tokenizer.Add(source);
        var tokens = tokenizer.Tokenize().ToList();

        Assert.Equal(13, tokens.Count);
        Assert.Equal(TokenType.Identifier, tokens[0].Type);
        Assert.Equal("f", tokens[0].Value);
        Assert.Equal(TokenType.OpeningParentheses, tokens[1].Type);
        Assert.Equal("(", tokens[1].Value);
        Assert.Equal(TokenType.Identifier, tokens[2].Type);
        Assert.Equal("variable", tokens[2].Value);
        Assert.Equal(TokenType.ClosingParentheses, tokens[3].Type);
        Assert.Equal(")", tokens[3].Value);
        Assert.Equal(TokenType.Whitespace, tokens[4].Type);
        Assert.Equal(" ", tokens[4].Value);
        Assert.Equal(TokenType.LineComment, tokens[5].Type);
        Assert.Equal("// this is a comment", tokens[5].Value);
        Assert.Equal(TokenType.NewLine, tokens[6].Type);
        Assert.Equal("\n", tokens[6].Value);
        Assert.Equal(TokenType.Identifier, tokens[7].Type);
        Assert.Equal("println", tokens[7].Value);
        Assert.Equal(TokenType.OpeningParentheses, tokens[8].Type);
        Assert.Equal("(", tokens[8].Value);
        Assert.Equal(TokenType.LiteralString, tokens[9].Type);
        Assert.Equal("text", tokens[9].Value);
        Assert.Equal(TokenType.ClosingParentheses, tokens[10].Type);
        Assert.Equal(")", tokens[10].Value);
        Assert.Equal(TokenType.Whitespace, tokens[11].Type);
        Assert.Equal(" ", tokens[11].Value);
        Assert.Equal(TokenType.LineComment, tokens[12].Type);
        Assert.Equal("// more comments", tokens[12].Value);
    }

    [Fact]
    public void TestTokenizeEmitsAnIdentifierBeforeALiteralString()
    {
        const string source = "identifier\"\"";

        var tokenizer = new Tokenizer();
        tokenizer.Add(source);
        var tokens = tokenizer.Tokenize().ToList();

        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.Identifier, tokens[0].Type);
        Assert.Equal("identifier", tokens[0].Value);
        Assert.Equal(TokenType.LiteralString, tokens[1].Type);
        Assert.Equal("", tokens[1].Value);
    }

    [Fact]
    public void TesetThrowsForInvalidIdentifiers()
    {
        const string source = "number a.b = 1";

        var tokenizer = new Tokenizer();
        tokenizer.Add(source);
        var exception = Assert.Throws<InvalidIdentifierException>(() => tokenizer.Tokenize().ToList());

        Assert.Equal("TOK001", exception.ErrorCode);
    }

    [Fact]
    public void TestThrowsForUnclosedStrings()
    {
        const string source = "\"string";

        var tokenizer = new Tokenizer();
        tokenizer.Add(source);
        var exception = Assert.Throws<UnterminatedStringException>(() => tokenizer.Tokenize().ToList());

        Assert.Equal("TOK002", exception.ErrorCode);
        Assert.Equal('\"', exception.StringDelimiter);
    }
}