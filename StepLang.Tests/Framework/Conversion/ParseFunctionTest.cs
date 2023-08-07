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
        var result = await function.EvaluateAsync(interpreter, new[] { ConstantExpression.String(targetType), value });

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task TestEvaluateThrowsForVoidValue()
    {
        var interpreter = new Interpreter();
        var function = new ParseFunction();

        await Assert.ThrowsAsync<InvalidResultTypeException>(() => function.EvaluateAsync(interpreter, new[] { ConstantExpression.String("text"), ConstantExpression.Void }));
    }

    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Used by xUnit")]
    private sealed class ParseData : TheoryData<string, Expression, ExpressionResult>
    {
        public ParseData()
        {
            Add("number", ConstantExpression.String("1.3"), ExpressionResult.Number(1.3));
            Add("number", ConstantExpression.True, ExpressionResult.Number(1));
            Add("number", ConstantExpression.False, ExpressionResult.Number(0));

            Add("string", ConstantExpression.Number(123.456), ExpressionResult.String("123.456"));
            Add("string", ConstantExpression.True, ExpressionResult.String("True"));

            Add("bool", ConstantExpression.String("true"), ExpressionResult.Bool(true));
            Add("bool", ConstantExpression.String("false"), ExpressionResult.Bool(false));
            Add("bool", ConstantExpression.String("1"), ExpressionResult.Bool(true));
            Add("bool", ConstantExpression.String("0"), ExpressionResult.Bool(false));
            Add("bool", ConstantExpression.Number(1), ExpressionResult.Bool(true));
            Add("bool", ConstantExpression.Number(0), ExpressionResult.Bool(false));

            Add("null", ConstantExpression.String("abc"), ExpressionResult.Null);
            Add("null", ConstantExpression.Number(123), ExpressionResult.Null);
            Add("null", ConstantExpression.True, ExpressionResult.Null);
            Add("any", ConstantExpression.Number(123), ExpressionResult.Null);
            Add("string", ConstantExpression.Null, ExpressionResult.Null);
            Add("number", ConstantExpression.Null, ExpressionResult.Null);
            Add("bool", ConstantExpression.Null, ExpressionResult.Null);

            Add("bool", ConstantExpression.True, ExpressionResult.Bool(true));
            Add("number", ConstantExpression.Number(123), ExpressionResult.Number(123));
            Add("string", ConstantExpression.String("abc"), ExpressionResult.String("abc"));
        }
    }
}