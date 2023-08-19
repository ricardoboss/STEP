using System.Diagnostics.CodeAnalysis;
using StepLang.Framework.Conversion;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Tests.Framework.IO;

public class LengthFunctionTest
{
    [Theory]
    [ClassData(typeof(ParseData))]
    public async Task TestEvaluateAsync(Expression value, NumberResult expected)
    {
        var interpreter = new Interpreter();
        var function = new LengthFunction();
        var result = await function.EvaluateAsync(interpreter, new []
        {
            value,
        });

        var numberResult = result.ExpectInteger();

        Assert.Equal(expected.Value, numberResult.Value);
    }

    [Fact]
    public async Task TestEvaluateThrowsForUnexpectedType()
    {
        var interpreter = new Interpreter();
        var function = new ParseFunction();

        await Assert.ThrowsAsync<InvalidOperationException>(() => function.EvaluateAsync(interpreter, new []
        {
            ConstantExpression.Number(0), ConstantExpression.Void,
        }));
    }

    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Used by xUnit")]
    private sealed class ParseData : TheoryData<Expression, NumberResult>
    {
        public ParseData()
        {
            Add(ConstantExpression.Str(""), new NumberResult(0));
            Add(ConstantExpression.Str("Hello"), new NumberResult(5));

            var list = new ListExpression(new List<Expression>
            {
                ConstantExpression.Str("A"),
                ConstantExpression.Number(1),
                ConstantExpression.Bool(true)
            });

            Add(list, new NumberResult(3));

            var constantList = ConstantExpression.List(new List<ExpressionResult>(new []
            {
                new NumberResult(123)
            }));
            Add(constantList, new NumberResult(1));

            var map = new MapExpression(new Dictionary<string, Expression>
            {
                {
                    "Foo", ConstantExpression.Str("A")
                },
                {
                    "Bar", ConstantExpression.Number(1)
                },
                {
                    "Baz", ConstantExpression.Bool(true)
                },
                {
                    "Bum", ConstantExpression.Str("lol")
                }
            });

            Add(map, new NumberResult(4));
        }
    }
}