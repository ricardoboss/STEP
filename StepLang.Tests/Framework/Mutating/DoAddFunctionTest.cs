using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions.Results;
using StepLang.Framework.Mutating;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Tests.Framework.Mutating;

public class DoAddFunctionTest
{
    [Theory]
    [ClassData(typeof(DoAddData))]
    public void TestDoAdd(ListResult list, ExpressionNode elementExpression)
    {
        const string listIdentifier = "mylist";

        var interpreter = new Interpreter();
        interpreter.CurrentScope.CreateVariable(listIdentifier, list);
        var listVarExpression = new IdentifierExpressionNode(new(TokenType.Identifier, listIdentifier));
        var function = new DoAddFunction();

        var previousCount = list.Value.Count;

        var result = function.Invoke(new(), interpreter, new[] { listVarExpression, elementExpression });

        Assert.Equal(VoidResult.Instance, result);
        Assert.Equal(previousCount + 1, list.Value.Count);
    }

    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Used by xUnit")]
    private sealed class DoAddData : TheoryData<ListResult, ExpressionNode>
    {
        public DoAddData()
        {
            Add(ListResult.Empty, LiteralExpressionNode.FromString("test"));
            Add(ListResult.Empty, LiteralExpressionNode.FromInt32(1));
            Add(ListResult.Empty, LiteralExpressionNode.FromBoolean(true));
            Add(ListResult.Empty, new AddExpressionNode(new(), LiteralExpressionNode.FromInt32(1), LiteralExpressionNode.FromInt32(2)));
            Add(TestHelper.ListFrom(BoolResult.True), LiteralExpressionNode.FromBoolean(false));
        }
    }
}