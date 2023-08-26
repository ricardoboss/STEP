using StepLang.Interpreting;
using StepLang.Parsing.Expressions;
using StepLang.Parsing.Statements;

namespace StepLang.Tests.Interpreting;

public class InterpretationTest
{
    [Fact]
    public async Task TestInvalidArgumentCountThrows()
    {
        var statements = await """
                               function add = (number a, number b) {
                                   return a + b
                               }

                               number result = add(1, 2, 3)
                               """.AsStatementsAsync();
        var interpreter = new Interpreter();
        var exception = await Assert.ThrowsAsync<InvalidFunctionCallException>(async () => await interpreter.InterpretAsync(statements.ToAsyncEnumerable()));

        Assert.Equal("INT002", exception.ErrorCode);
        Assert.IsType<InvalidArgumentCountException>(exception.InnerException);
    }

    [Fact]
    public async Task TestInvalidValueAssignmentThrows()
    {
        var statement = await "number a = \"Hello\"".AsStatementAsync();
        var interpreter = new Interpreter();
        var exception = await Assert.ThrowsAsync<InvalidVariableAssignmentException>(async () => await statement.ExecuteAsync(interpreter));

        Assert.Equal("TYP001", exception.ErrorCode);
    }

    [Fact]
    public async Task TestAddWithBoolsThrowsException()
    {
        var statement = await "string a = true + false".AsStatementAsync();
        var interpreter = new Interpreter();
        var exception = await Assert.ThrowsAsync<IncompatibleExpressionOperandsException>(async () => await statement.ExecuteAsync(interpreter));

        Assert.Equal("TYP003", exception.ErrorCode);
    }
}