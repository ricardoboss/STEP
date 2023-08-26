using StepLang.Parsing.Expressions;
using StepLang.Parsing.Statements;
using StepLang.Tokenizing;

namespace StepLang.Tests;

public static class TestHelper
{
    public static IEnumerable<Token> AsTokens(this string code)
    {
        var tokenizer = new Tokenizer();
        tokenizer.Add(code);
        return tokenizer.TokenizeAsync();
    }

    public static async Task<Expression> AsExpressionAsync(this string code)
    {
        return await ExpressionParser.ParseAsync(code.AsTokens(), CancellationToken.None);
    }

    public static async Task<List<Statement>> AsStatementsAsync(this string code)
    {
        var parser = new StatementParser();
        parser.Add(code.AsTokens());
        return await parser.ParseAsync(CancellationToken.None).ToListAsync();
    }

    public static async Task<Statement> AsStatementAsync(this string code)
    {
        var statements = await code.AsStatementsAsync();

        return statements.Single();
    }
}