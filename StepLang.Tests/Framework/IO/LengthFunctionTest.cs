using System.Diagnostics.CodeAnalysis;
using StepLang.Framework.Conversion;
using StepLang.Framework.IO;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Tests.Framework.IO;

public class LengthFunctionTest
{
    [Theory]
    [ClassData(typeof(ParseData))]
    public async Task TestEvaluateAsync(Expression value, ExpressionResult expected)
    {
        var interpreter = new Interpreter();
        var function = new LengthFunction();
        var result = await function.EvaluateAsync(interpreter, new []
        {
            value,
        });

        Assert.Equal(expected, result);
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
    private sealed class ParseData : TheoryData<Expression, ExpressionResult>
    {
        public ParseData()
        {
            Add(ConstantExpression.Str(""), new NumberResult(0));
            Add(ConstantExpression.Str("Hello"), new NumberResult(5));
        }
    }
}