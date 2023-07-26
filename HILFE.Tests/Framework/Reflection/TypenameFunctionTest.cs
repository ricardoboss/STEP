using HILFE.Framework.Reflection;
using HILFE.Interpreting;
using HILFE.Parsing.Expressions;
using HILFE.Tokenizing;

namespace HILFE.Tests.Framework.Reflection;

public class TypenameFunctionTest
{
    [Theory]
    [ClassData(typeof(TypenameData))]
    public async Task TestEvaluateAsync(string typeName, ExpressionResult arg)
    {
        const string varName = "a";
        var interpreter = new Interpreter();
        interpreter.CurrentScope.SetVariable(varName, arg);
        
        var function = new TypenameFunction();
        var result = await function.EvaluateAsync(interpreter, new []{ new VariableExpression(new(TokenType.Identifier, varName)) });

        Assert.Equal("string", result.ValueType);
        Assert.Equal(typeName, result.Value as string);
    }

    [Fact]
    public async Task TestEvaluateAsyncThrowsForNonVariableExpression()
    {
        var interpreter = new Interpreter();
        var function = new TypenameFunction();
        
        await Assert.ThrowsAsync<InvalidExpressionTypeException>(() => function.EvaluateAsync(interpreter, new []{ ConstantExpression.String("abc") }));
    }

    [Fact]
    public async Task TestEvaluateAsyncThrowsForInvalidArgCounts()
    {
        var interpreter = new Interpreter();
        var function = new TypenameFunction();
        
        await Assert.ThrowsAsync<InvalidArgumentCountException>(() => function.EvaluateAsync(interpreter, new []{ ConstantExpression.String("abc"), ConstantExpression.String("abc") }));
        await Assert.ThrowsAsync<InvalidArgumentCountException>(() => function.EvaluateAsync(interpreter, Array.Empty<Expression>()));
    }

    private class TypenameData : TheoryData<string, ExpressionResult>
    {
        public TypenameData()
        {
            Add("null", ExpressionResult.Null);
            Add("void", ExpressionResult.Void);
            Add("bool", ExpressionResult.True);
            Add("number", ExpressionResult.Number(123));
            Add("string", ExpressionResult.String("test"));
            Add("function", ExpressionResult.Function(new TypenameFunction()));
        }
    }
}