using HILFE.Parsing;
using HILFE.Tokenizing;

namespace HILFE.Tests.Parsing;

public class ParserTest
{
    [Fact]
    public async Task TestParseFunctionCallNoParams()
    {
        var parser = new Parser();
        var statements = await parser.ParseAsync(new Token[] { new(TokenType.Identifier, "function"), new(TokenType.ExpressionOpener, "("), new(TokenType.ExpressionCloser, ")") }).ToListAsync();

        Assert.Single(statements);
        Assert.Equal(StatementType.FunctionCall, statements[0].Type);
    }

    [Fact]
    public async Task TestParseUnexpectedTokenThrows()
    {
        var parser = new Parser();
        await Assert.ThrowsAsync<UnexpectedTokenException>(async () => await parser.ParseAsync(new Token[] { new(TokenType.BreakKeyword, "break") }).ToListAsync());
    }

    [Fact]
    public async Task TestParseImbalancedCodeBlocksThrows()
    {
        await Assert.ThrowsAsync<ImbalancedCodeBlocksException>(async () => {
            var parser = new Parser();
            var statements = await parser.ParseAsync(new Token []
            {
                new(TokenType.CodeBlockOpener, "{"), new(TokenType.CodeBlockOpener, "{"),
                new(TokenType.CodeBlockCloser, "}"),
            }).ToListAsync();

            Assert.Equal(3, statements.Count);
            Assert.Equal(StatementType.CodeBlockStart, statements[0].Type);
            Assert.Equal(StatementType.CodeBlockStart, statements[1].Type);
            Assert.Equal(StatementType.CodeBlockEnd, statements[2].Type);

            parser.End();
        });
    }

    [Fact]
    public async Task TestParseMultipleParseCalls()
    {
        var parser = new Parser();
        var statements = await parser.ParseAsync(new Token []
        {
            new(TokenType.CodeBlockOpener, "{"), new(TokenType.CodeBlockOpener, "{"),
            new(TokenType.CodeBlockCloser, "}"),
        }).ToListAsync();

        Assert.Equal(3, statements.Count);
        Assert.Equal(StatementType.CodeBlockStart, statements[0].Type);
        Assert.Equal(StatementType.CodeBlockStart, statements[1].Type);
        Assert.Equal(StatementType.CodeBlockEnd, statements[2].Type);

        var statements2 = await parser.ParseAsync(new Token []
        {
            new(TokenType.CodeBlockCloser, "}"),
        }).ToListAsync();

        Assert.Single(statements2);
        Assert.Equal(StatementType.CodeBlockEnd, statements2[0].Type);
    }
}