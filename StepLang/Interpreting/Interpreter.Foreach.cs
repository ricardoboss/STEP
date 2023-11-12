using System.Diagnostics;
using StepLang.Expressions.Results;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public void Execute(ForeachDeclareKeyDeclareValueStatementNode statementNode)
    {
        var keyVariable = statementNode.KeyDeclaration.EvaluateUsing(this);
        var keyLocation = statementNode.KeyDeclaration.Types.First().Location;
        var valueVariable = statementNode.ValueDeclaration.EvaluateUsing(this);
        var valueLocation = statementNode.ValueDeclaration.Types.First().Location;
        var collection = statementNode.Collection.EvaluateUsing(this);

        RunForeachLoop(keyLocation, keyVariable, valueLocation, valueVariable, collection, statementNode.Body);
    }

    public void Execute(ForeachDeclareKeyValueStatementNode statementNode)
    {
        var keyVariable = statementNode.KeyDeclaration.EvaluateUsing(this);
        var keyLocation = statementNode.KeyDeclaration.Types.First().Location;
        var valueVariable = CurrentScope.GetVariable(statementNode.ValueIdentifier);
        var valueLocation = statementNode.ValueIdentifier.Location;
        var collection = statementNode.Collection.EvaluateUsing(this);

        RunForeachLoop(keyLocation, keyVariable, valueLocation, valueVariable, collection, statementNode.Body);
    }

    public void Execute(ForeachDeclareValueStatementNode statementNode)
    {
        var valueVariable = statementNode.ValueDeclaration.EvaluateUsing(this);
        var valueLocation = statementNode.ValueDeclaration.Types.First().Location;
        var collection = statementNode.Collection.EvaluateUsing(this);

        RunForeachLoop(null, null, valueLocation, valueVariable, collection, statementNode.Body);
    }

    public void Execute(ForeachKeyValueStatementNode statementNode)
    {
        var keyVariable = CurrentScope.GetVariable(statementNode.KeyIdentifier);
        var keyLocation = statementNode.KeyIdentifier.Location;
        var valueVariable = CurrentScope.GetVariable(statementNode.ValueIdentifier);
        var valueLocation = statementNode.ValueIdentifier.Location;
        var collection = statementNode.Collection.EvaluateUsing(this);

        RunForeachLoop(keyLocation, keyVariable, valueLocation, valueVariable, collection, statementNode.Body);
    }

    public void Execute(ForeachKeyDeclareValueStatementNode statementNode)
    {
        var keyVariable = CurrentScope.GetVariable(statementNode.KeyIdentifier);
        var keyLocation = statementNode.KeyIdentifier.Location;
        var valueVariable = statementNode.ValueDeclaration.EvaluateUsing(this);
        var valueLocation = statementNode.ValueDeclaration.Types.First().Location;
        var collection = statementNode.Collection.EvaluateUsing(this);

        RunForeachLoop(keyLocation, keyVariable, valueLocation, valueVariable, collection, statementNode.Body);
    }

    public void Execute(ForeachValueStatementNode statementNode)
    {
        var valueVariable = CurrentScope.GetVariable(statementNode.Identifier);
        var valueLocation = statementNode.Identifier.Location;
        var collection = statementNode.Collection.EvaluateUsing(this);

        RunForeachLoop(null, null, valueLocation, valueVariable, collection, statementNode.Body);
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

    private void RunForeachLoop(TokenLocation? keyAssignmentLocation, Variable? key, TokenLocation valueAssignmentLocation, Variable value, ExpressionResult collection, IReadOnlyCollection<StatementNode> body)
    {
        Debug.Assert(keyAssignmentLocation is null || key is not null);

        var pairs = ConvertToForeachEnumerable(collection);

        foreach (var (keyValue, valueValue) in pairs)
        {
            PushScope();

            key?.Assign(keyAssignmentLocation!, keyValue);
            value.Assign(valueAssignmentLocation, valueValue);

            Execute(body);

            if (BreakDepth > 0)
            {
                BreakDepth--;

                PopScope();

                break;
            }

            if (CurrentScope.TryGetResult(out var resultValue, out var resultLocation))
            {
                PopScope();

                CurrentScope.SetResult(resultLocation, resultValue);

                break;
            }
        }
    }
}