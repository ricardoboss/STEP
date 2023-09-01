using System.Diagnostics.CodeAnalysis;
using StepLang.Framework.Mutating;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Tests.Framework.Mutating;

public class DoAddFunctionTest
{
    [Theory]
    [ClassData(typeof(DoAddData))]
    public async Task TestDoAdd(ListResult list, Expression elementExpression)
    {
        const string listIdentifier = "mylist";

        var interpreter = new Interpreter();
        interpreter.CurrentScope.SetVariable(listIdentifier, list);
        var listVarExpression = new VariableExpression(new(TokenType.Identifier, listIdentifier, null));
        var function = new DoAddFunction();

        var previousCount = list.Value.Count;

        var result = await function.EvaluateAsync(interpreter, new[] { listVarExpression, elementExpression });

        Assert.Equal(VoidResult.Instance, result);
        Assert.Equal(previousCount + 1, list.Value.Count);
    }

    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Used by xUnit")]
    private sealed class DoAddData : TheoryData<ListResult, Expression>
    {
        public DoAddData()
        {
            Add(ListResult.Empty, ConstantExpression.Str("test"));
            Add(ListResult.Empty, ConstantExpression.Number(1));
            Add(ListResult.Empty, ConstantExpression.True);
            Add(ListResult.Empty, Expression.Add(ConstantExpression.Number(1), ConstantExpression.Number(2)));
            Add(ListResult.From(BoolResult.True), ConstantExpression.False);
        }
    }
}