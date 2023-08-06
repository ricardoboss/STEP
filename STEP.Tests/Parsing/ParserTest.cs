using STEP.Parsing;
using STEP.Parsing.Statements;
using STEP.Tokenizing;

namespace STEP.Tests.Parsing;

public class ParserTest
{
    [Fact]
    public async Task TestParseFunctionCallNoParams()
    {
        var parser = new StatementParser();
        parser.Add(new Token []
        {
            new(TokenType.Identifier, "function"),
            new(TokenType.OpeningParentheses, "("),
            new(TokenType.ClosingParentheses, ")"),
        });

        var statements = await parser.ParseAsync().ToListAsync();

        Assert.Single(statements);
        Assert.Equal(StatementType.FunctionCall, statements[0].Type);
    }

    [Fact]
    public async Task TestParseUnexpectedTokenThrows()
    {
        var parser = new StatementParser();
        parser.Add(new Token []
        {
            new(TokenType.LiteralString, "string"),
        });

        await Assert.ThrowsAsync<UnexpectedTokenException>(async () => await parser.ParseAsync().ToListAsync());
    }

    // [Fact]
    // public async Task TestParseImbalancedCodeBlocksThrows()
    // {
    //     await Assert.ThrowsAsync<ImbalancedCodeBlocksException>(async () =>
    //     {
    //         var parser = new StatementParser();
    //         parser.Add(new Token []
    //         {
    //             new(TokenType.OpeningCurlyBracket, "{"), new(TokenType.OpeningCurlyBracket, "{"),
    //             new(TokenType.ClosingCurlyBracket, "}"),
    //         });
    //
    //         var statements = await parser.ParseAsync().ToListAsync();
    //
    //         Assert.True(true);
    //     });
    // }

    // [Fact]
    // public async Task TestParseMultipleParseCalls()
    // {
    //     var parser = new StatementParser();
    //     var statements = await parser.ParseAsync(new Token []
    //     {
    //         new(TokenType.OpeningCurlyBracket, "{"), new(TokenType.OpeningCurlyBracket, "{"),
    //         new(TokenType.ClosingCurlyBracket, "}"),
    //     }).ToListAsync();
    //
    //     Assert.Equal(3, statements.Count);
    //     Assert.Equal(StatementType.CodeBlockStart, statements[0].Type);
    //     Assert.Equal(StatementType.CodeBlockStart, statements[1].Type);
    //     Assert.Equal(StatementType.CodeBlockEnd, statements[2].Type);
    //
    //     var statements2 = await parser.ParseAsync(new Token []
    //     {
    //         new(TokenType.ClosingCurlyBracket, "}"),
    //     }).ToListAsync();
    //
    //     Assert.Single(statements2);
    //     Assert.Equal(StatementType.CodeBlockEnd, statements2[0].Type);
    // }
}