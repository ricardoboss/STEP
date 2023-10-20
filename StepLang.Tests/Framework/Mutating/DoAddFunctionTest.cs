using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Framework.Mutating;
using StepLang.Interpreting;
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
        interpreter.CurrentScope.CreateVariable(listIdentifier, list);
        var listVarExpression = new VariableExpression(new(TokenType.Identifier, listIdentifier));
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
            Add(ListResult.Empty, LiteralExpression.Str("test"));
            Add(ListResult.Empty, LiteralExpression.Number(1));
            Add(ListResult.Empty, LiteralExpression.True);
            Add(ListResult.Empty, new AddExpression(LiteralExpression.Number(1), LiteralExpression.Number(2)));
            Add(ListResult.From(BoolResult.True), LiteralExpression.False);
        }
    }
}