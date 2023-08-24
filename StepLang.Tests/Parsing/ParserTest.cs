using StepLang.Parsing;
using StepLang.Parsing.Statements;
using StepLang.Tokenizing;

namespace StepLang.Tests.Parsing;

public class ParserTest
{
    [Fact]
    public async Task TestParseFunctionCallNoParams()
    {
        var parser = new StatementParser();
        parser.Add(new Token[]
        {
            new(TokenType.Identifier, "function", null),
            new(TokenType.OpeningParentheses, "(", null),
            new(TokenType.ClosingParentheses, ")", null),
        });

        var statements = await parser.ParseAsync().ToListAsync();

        Assert.Single(statements);
        Assert.Equal(StatementType.FunctionCall, statements[0].Type);
    }

    [Fact]
    public async Task TestParseUnexpectedTokenThrows()
    {
        var parser = new StatementParser();
        parser.Add(new Token[]
        {
            new(TokenType.LiteralString, "string", null),
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
    //             new(TokenType.OpeningCurlyBracket, "{", null), new(TokenType.OpeningCurlyBracket, "{", null),
    //             new(TokenType.ClosingCurlyBracket, "}", null),
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
    //         new(TokenType.OpeningCurlyBracket, "{", null), new(TokenType.OpeningCurlyBracket, "{", null),
    //         new(TokenType.ClosingCurlyBracket, "}", null),
    //     }).ToListAsync();
    //
    //     Assert.Equal(3, statements.Count);
    //     Assert.Equal(StatementType.CodeBlockStart, statements[0].Type);
    //     Assert.Equal(StatementType.CodeBlockStart, statements[1].Type);
    //     Assert.Equal(StatementType.CodeBlockEnd, statements[2].Type);
    //
    //     var statements2 = await parser.ParseAsync(new Token []
    //     {
    //         new(TokenType.ClosingCurlyBracket, "}", null),
    //     }).ToListAsync();
    //
    //     Assert.Single(statements2);
    //     Assert.Equal(StatementType.CodeBlockEnd, statements2[0].Type);
    // }
}