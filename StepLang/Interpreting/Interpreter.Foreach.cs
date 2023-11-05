using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public void Execute(ForeachDeclareKeyDeclareValueStatementNode statementNode)
    {
        var keyVariable = statementNode.KeyDeclaration.EvaluateUsing(this);
        var valueVariable = statementNode.ValueDeclaration.EvaluateUsing(this);
        var collection = statementNode.Collection.EvaluateUsing(this);

        RunForeachLoop(keyVariable, valueVariable, collection, statementNode.Body);
    }

    public void Execute(ForeachDeclareKeyValueStatementNode statementNode)
    {
        var keyVariable = statementNode.KeyDeclaration.EvaluateUsing(this);
        var valueVariable = CurrentScope.GetVariable(statementNode.ValueIdentifier);
        var collection = statementNode.Collection.EvaluateUsing(this);

        RunForeachLoop(keyVariable, valueVariable, collection, statementNode.Body);
    }

    public void Execute(ForeachDeclareValueStatementNode statementNode)
    {
        var valueVariable = statementNode.ValueDeclaration.EvaluateUsing(this);
        var collection = statementNode.Collection.EvaluateUsing(this);

        RunForeachLoop(null, valueVariable, collection, statementNode.Body);
    }

    public void Execute(ForeachKeyValueStatementNode statementNode)
    {
        var keyVariable = CurrentScope.GetVariable(statementNode.KeyIdentifier);
        var valueVariable = CurrentScope.GetVariable(statementNode.ValueIdentifier);
        var collection = statementNode.Collection.EvaluateUsing(this);

        RunForeachLoop(keyVariable, valueVariable, collection, statementNode.Body);
    }

    public void Execute(ForeachKeyDeclareValueStatementNode statementNode)
    {
        var keyVariable = CurrentScope.GetVariable(statementNode.KeyIdentifier);
        var valueVariable = statementNode.ValueDeclaration.EvaluateUsing(this);
        var collection = statementNode.Collection.EvaluateUsing(this);

        RunForeachLoop(keyVariable, valueVariable, collection, statementNode.Body);
    }

    public void Execute(ForeachValueStatementNode statementNode)
    {
        var valueVariable = CurrentScope.GetVariable(statementNode.Identifier);
        var collection = statementNode.Collection.EvaluateUsing(this);

        RunForeachLoop(null, valueVariable, collection, statementNode.Body);
    }

    private static IEnumerable<(ExpressionResult, ExpressionResult)> ConvertToForeachEnumerable(ExpressionResult collection)
    {
        switch (collection)
        {
            case ListResult { Value: var items }:
            {
                var index = 0;
                foreach (var item in items)
                {
                    yield return (new NumberResult(index), item);

                    index++;
                }

                yield break;
            }
            case MapResult { Value: var pairs }:
            {
                foreach (var (key, value) in pairs)
                    yield return (new StringResult(key), value);

                yield break;
            }
            default:
                throw new InvalidResultTypeException(collection, ResultType.List, ResultType.Map);
        }
    }

    private void RunForeachLoop(Variable? key, Variable value, ExpressionResult collection, IReadOnlyCollection<StatementNode> body)
    {
        var pairs = ConvertToForeachEnumerable(collection);

        foreach (var (keyValue, valueValue) in pairs)
        {
            PushScope();

            key?.Assign(keyValue);
            value.Assign(valueValue);

            Execute(body);

            if (BreakDepth > 0)
            {
                BreakDepth--;

                PopScope();

                break;
            }

            if (CurrentScope.TryGetResult(out _))
            {
                PopScope();

                break;
            }
        }
    }
}