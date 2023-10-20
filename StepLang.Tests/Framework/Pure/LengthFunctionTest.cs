using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Framework.Pure;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Tests.Framework.Pure;

public class LengthFunctionTest
{
    [Theory]
    [ClassData(typeof(LengthData))]
    public async Task TestEvaluateAsync(Expression value, NumberResult expected)
    {
        var interpreter = new Interpreter();
        var function = new LengthFunction();
        var result = await function.EvaluateAsync(interpreter, new[] { value });

        var numberResult = result.ExpectInteger();

        Assert.Equal(expected.Value, numberResult.Value);
    }

    [Fact]
    public async Task TestEvaluateWithVariableAsync()
    {
        var interpreter = new Interpreter();
        interpreter.CurrentScope.CreateVariable("foo", LiteralExpression.Str("Hello").Result);

        var function = new LengthFunction();
        var result = await function.EvaluateAsync(interpreter, new[] { new VariableExpression(new(TokenType.Identifier, "foo")) });

        var numberResult = result.ExpectInteger();

        Assert.Equal(5, numberResult.Value);
    }

    [Fact]
    public async Task TestEvaluateThrowsForUnexpectedType()
    {
        var interpreter = new Interpreter();
        var function = new LengthFunction();

        await Assert.ThrowsAsync<InvalidArgumentTypeException>(() => function.EvaluateAsync(interpreter, new[] { LiteralExpression.Number(0) }));
    }

    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Used by xUnit")]
    private sealed class LengthData : TheoryData<Expression, NumberResult>
    {
        public LengthData()
        {
            Add(LiteralExpression.Str(""), new NumberResult(0));
            Add(LiteralExpression.Str("Hello"), new NumberResult(5));

            var list = new ListExpression(new List<Expression>
            {
                LiteralExpression.Str("A"),
                LiteralExpression.Number(1),
                LiteralExpression.Bool(true),
            });

            Add(list, new NumberResult(3));

            var constantList = LiteralExpression.List(new List<ExpressionResult>(new[]
            {
                new NumberResult(123),
            }));
            Add(constantList, new NumberResult(1));

            var map = new MapExpression(new Dictionary<string, Expression>
            {
                {
                    "Foo", LiteralExpression.Str("A")
                },
                {
                    "Bar", LiteralExpression.Number(1)
                },
                {
                    "Baz", LiteralExpression.Bool(true)
                },
                {
                    "Bum", LiteralExpression.Str("lol")
                },
            });

            Add(map, new NumberResult(4));
        }
    }
}