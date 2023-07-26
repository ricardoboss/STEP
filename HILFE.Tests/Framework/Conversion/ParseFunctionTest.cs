using HILFE.Framework.Conversion;
using HILFE.Interpreting;
using HILFE.Parsing.Expressions;

namespace HILFE.Tests.Framework.Conversion;

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

    private class ParseData : TheoryData<string, Expression, ExpressionResult>
    {
        public ParseData()
        {
            Add("number", ConstantExpression.String("1.3"), new("number", 1.3));
            Add("number", ConstantExpression.True, new("number", 1));
            Add("number", ConstantExpression.False, new("number", 0));

            Add("string", ConstantExpression.Number(123.456), new("string", "123.456"));
            Add("string", ConstantExpression.True, new("string", "True"));

            Add("bool", ConstantExpression.String("true"), new("bool", true));
            Add("bool", ConstantExpression.String("false"), new("bool", false));
            Add("bool", ConstantExpression.String("1"), new("bool", true));
            Add("bool", ConstantExpression.String("0"), new("bool", false));
            Add("bool", ConstantExpression.Number(1), new("bool", true));
            Add("bool", ConstantExpression.Number(0), new("bool", false));

            Add("null", ConstantExpression.String("abc"), ExpressionResult.Null);
            Add("null", ConstantExpression.Number(123), ExpressionResult.Null);
            Add("null", ConstantExpression.True, ExpressionResult.Null);
            Add("any", ConstantExpression.Number(123), ExpressionResult.Null);
            Add("string", ConstantExpression.Null, ExpressionResult.Null);
            Add("number", ConstantExpression.Null, ExpressionResult.Null);
            Add("bool", ConstantExpression.Null, ExpressionResult.Null);

            Add("bool", ConstantExpression.True, new("bool", true));
            Add("number", ConstantExpression.Number(123), new("number", 123));
            Add("string", ConstantExpression.String("abc"), new("string", "abc"));
        }
    }
}