using HILFE.Parsing;
using HILFE.Tokenizing;

namespace HILFE.Tests.Parsing;

public class ParserTest
{
    [Fact]
    public async Task TestParseFunctionCallNoParams()
    {
        var statements = await Parser.ParseAsync(new Token[] { new(TokenType.Identifier, "function"), new(TokenType.ExpressionOpener, "("), new(TokenType.ExpressionCloser, ")") }).ToListAsync();

        Assert.Single(statements);
        Assert.Equal(StatementType.FunctionCall, statements[0].Type);
    }

    [Fact]
    public async Task TestParseUnexpectedTokenThrows()
    {
        await Assert.ThrowsAsync<UnexpectedTokenException>(async () => await Parser.ParseAsync(new Token[] { new(TokenType.BreakKeyword, "break") }).ToListAsync());
    }

    [Fact]
    public async Task TestParseImbalancedCodeBlocksThrows()
    {
        await Assert.ThrowsAsync<ImbalancedCodeBlocksException>(async () => await Parser.ParseAsync(new Token[] { new(TokenType.CodeBlockOpener, "{"), new(TokenType.CodeBlockOpener, "{"), new(TokenType.CodeBlockCloser, "}") }).ToListAsync());
    }
}