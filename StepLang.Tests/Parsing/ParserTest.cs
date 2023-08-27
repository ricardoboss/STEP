using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Parsing.Expressions;
using StepLang.Parsing.Statements;

namespace StepLang.Tests.Parsing;

public class ParserTest
{
    [Fact]
    public async Task TestParseFunctionCallNoParams()
    {
        var parser = "func()".AsParsable();

        var statements = await parser.ParseAsync().ToListAsync();

        Assert.Single(statements);
        Assert.Equal(StatementType.FunctionCall, statements[0].Type);
    }

    [Fact]
    public async Task TestParseUnexpectedTokenThrows()
    {
        var parser = "\"string\"".AsParsable();

        var exception = await Assert.ThrowsAsync<UnexpectedTokenException>(async () => await parser.ParseAsync().ToListAsync());

        Assert.Equal("PAR001", exception.ErrorCode);
    }

    [Fact]
    public async Task TestMissingValueThrows()
    {
        var parser = "number a = ".AsParsable();

        var exception = await Assert.ThrowsAsync<MissingValueExpressionException>(async () => await parser.ParseAsync().ToListAsync());

        Assert.Equal("PAR003", exception.ErrorCode);
    }

    [Fact]
    public async Task TestMissingConditionThrows()
    {
        var parser = """
                     if () {
                         print("Hello World!")
                     }
                     """.AsParsable();

        var exception = await Assert.ThrowsAsync<MissingConditionExpressionException>(async () => await parser.ParseAsync().ToListAsync());

        Assert.Equal("PAR004", exception.ErrorCode);
    }

    [Fact]
    public async Task TestImportedFileDoesNotExistThrows()
    {
        var statement = await "import \"doesnotexist.step\"".AsStatementAsync();
        var interpreter = new Interpreter();

        var exception = await Assert.ThrowsAsync<ImportedFileDoesNotExistException>(async () => await statement.ExecuteAsync(interpreter));

        Assert.Equal("PAR005", exception.ErrorCode);
    }

    [Fact]
    public async Task TestImportStatementNoLongerAllowedThrows()
    {
        var parser = """
                     // this is a comment
                     import "somefile.step"

                     number x = 1

                     import "someotherfile.step"
                     """.AsParsable();

        var exception = await Assert.ThrowsAsync<ImportsNoLongerAllowedException>(async () => await parser.ParseAsync().ToListAsync());

        Assert.Equal("PAR007", exception.ErrorCode);
    }

    [Fact]
    public async Task TestInvalidIndexOperatorThrows()
    {
        var statements = await """
                               number a = 1

                               println(a[0])
                               """.AsStatementsAsync();

        var output = new StringWriter();
        var interpreter = new Interpreter(output);

        var exception = await Assert.ThrowsAsync<InvalidIndexOperatorException>(async () => await interpreter.InterpretAsync(statements.ToAsyncEnumerable()));

        Assert.Equal("PAR009", exception.ErrorCode);
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