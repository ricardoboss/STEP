using StepLang.Interpreting;
using StepLang.Parsing.Expressions;
using StepLang.Parsing.Statements;

namespace StepLang.Tests.Interpreting;

public class InterpretationTest
{
    [Fact]
    public async Task TestUndefinedIdentifierThrows()
    {
        var statement = await "number a = b + 1".AsStatementAsync();
        var interpreter = new Interpreter();
        var exception = await Assert.ThrowsAsync<UndefinedIdentifierException>(async () => await statement.ExecuteAsync(interpreter));

        Assert.Equal("INT001", exception.ErrorCode);
    }

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
    public async Task TestInvalidDepthThrows()
    {
        var statements = await "break 1 - 2".AsStatementsAsync();
        var interpreter = new Interpreter();
        var exception = await Assert.ThrowsAsync<InvalidDepthResultException>(async () => await interpreter.InterpretAsync(statements.ToAsyncEnumerable()));

        Assert.Equal("INT003", exception.ErrorCode);
    }

    [Fact]
    public async Task TestInvalidExpressionTypeThrows()
    {
        var statements = await "toTypeName(1)".AsStatementsAsync();
        var interpreter = new Interpreter();
        var exception = await Assert.ThrowsAsync<InvalidExpressionTypeException>(async () => await interpreter.InterpretAsync(statements.ToAsyncEnumerable()));

        Assert.Equal("INT004", exception.ErrorCode);
    }

    [Fact]
    public async Task TestListIndexOutOfBoundsThrows()
    {
        var statements = await """
                               list k = [1, 2, 3]
                               doRemoveAt(k, 3)
                               """.AsStatementsAsync();
        var interpreter = new Interpreter();
        var exception = await Assert.ThrowsAsync<IndexOutOfBoundsException>(async () => await interpreter.InterpretAsync(statements.ToAsyncEnumerable()));

        Assert.Equal("INT005", exception.ErrorCode);
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

        Assert.Equal("TYP002", exception.ErrorCode);
    }

    [Fact]
    public async Task TestInvalidArgumentTypeThrows()
    {
        var statements = await """
                              function add = (number a, number b) {
                                  return a + b
                              }

                              add(1, "2")
                              """.AsStatementsAsync();
        var interpreter = new Interpreter();
        var exception = await Assert.ThrowsAsync<InvalidFunctionCallException>(async () => await interpreter.InterpretAsync(statements.ToAsyncEnumerable()));

        Assert.Equal("TYP003", exception.ErrorCode);
        Assert.IsType<InvalidArgumentTypeException>(exception.InnerException);
    }

    [Fact]
    public async Task TestInvalidResultTypeThrows()
    {
        var statements = await """
                               function add = (number a, number b) {
                                   return a + b
                               }

                               add(1, 2)
                               """.AsStatementsAsync();
        var interpreter = new Interpreter();
        var exception = await Assert.ThrowsAsync<InvalidResultTypeException>(async () => await interpreter.InterpretAsync(statements.ToAsyncEnumerable()));

        Assert.Equal("TYP004", exception.ErrorCode);
    }
}