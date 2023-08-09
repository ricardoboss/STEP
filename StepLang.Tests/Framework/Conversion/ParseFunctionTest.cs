using System.Diagnostics.CodeAnalysis;
using StepLang.Framework.Conversion;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Tests.Framework.Conversion;

public class ParseFunctionTest
{
    [Theory]
    [ClassData(typeof(ParseData))]
    public async Task TestEvaluateAsync(string targetType, Expression value, ExpressionResult expected)
    {
        var interpreter = new Interpreter();
        var function = new ParseFunction();
        var result = await function.EvaluateAsync(interpreter, new[] { ConstantExpression.Str(targetType), value });

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task TestEvaluateThrowsForVoidValue()
    {
        var interpreter = new Interpreter();
        var function = new ParseFunction();

        await Assert.ThrowsAsync<InvalidResultTypeException>(() => function.EvaluateAsync(interpreter, new[] { ConstantExpression.Str("string"), ConstantExpression.Void }));
    }

    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Used by xUnit")]
    private sealed class ParseData : TheoryData<string, Expression, ExpressionResult>
    {
        public ParseData()
        {
            Add("number", ConstantExpression.Str("1.3"), new NumberResult(1.3));
            Add("number", ConstantExpression.True, new NumberResult(1));
            Add("number", ConstantExpression.False, new NumberResult(0));

            Add("string", ConstantExpression.Number(123.456), new StringResult("123.456"));
            Add("string", ConstantExpression.True, new StringResult("True"));

            Add("bool", ConstantExpression.Str("true"), BoolResult.True);
            Add("bool", ConstantExpression.Str("false"), BoolResult.False);
            Add("bool", ConstantExpression.Str("1"), BoolResult.True);
            Add("bool", ConstantExpression.Str("0"), BoolResult.False);
            Add("bool", ConstantExpression.Number(1), BoolResult.True);
            Add("bool", ConstantExpression.Number(0), BoolResult.False);

            Add("null", ConstantExpression.Str("abc"), NullResult.Instance);
            Add("null", ConstantExpression.Number(123), NullResult.Instance);
            Add("null", ConstantExpression.True, NullResult.Instance);

            Add("string", ConstantExpression.Null, StringResult.Empty);
            Add("number", ConstantExpression.Null, NumberResult.Zero);
            Add("bool", ConstantExpression.Null, BoolResult.False);

            Add("bool", ConstantExpression.True, BoolResult.True);
            Add("number", ConstantExpression.Number(123), new NumberResult(123));
            Add("string", ConstantExpression.Str("abc"), new StringResult("abc"));
        }
    }
}