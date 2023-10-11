using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Statements;

public class ForEachStatement : Statement
{
    private readonly Expression iterableExpression;
    private readonly Expression? keyVariableExpression;
    private readonly Expression valueVariableExpression;
    private readonly IReadOnlyList<Statement> statements;

    /// <inheritdoc />
    public ForEachStatement(Expression? keyVariableExpression, Expression valueVariableExpression, Expression iterableExpression, IReadOnlyList<Statement> statements) : base(StatementType.WhileStatement)
    {
        this.iterableExpression = iterableExpression;
        this.keyVariableExpression = keyVariableExpression;
        this.valueVariableExpression = valueVariableExpression;
        this.statements = statements;
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        Token? keyToken = null;
        Token valueToken;

        if (keyVariableExpression is not null)
        {
            if (keyVariableExpression is VariableExpression keyVariable)
                keyToken = keyVariable.Identifier;
            else if (keyVariableExpression is VariableDeclarationExpression keyDeclaration)
            {
                await keyDeclaration.EvaluateAsync(interpreter, cancellationToken);

                keyToken = keyDeclaration.Identifier;
            }
            else
                throw new InvalidExpressionException(null, "Key expression must be a variable.");
        }

        if (valueVariableExpression is VariableExpression valueVariable)
            valueToken = valueVariable.Identifier;
        else if (valueVariableExpression is VariableDeclarationExpression valueDeclaration)
        {
            await valueDeclaration.EvaluateAsync(interpreter, cancellationToken);

            valueToken = valueDeclaration.Identifier;
        }
        else
            throw new InvalidExpressionException(null, "Value expression must be a variable.");

        var iterableResult = await iterableExpression.EvaluateAsync(interpreter, cancellationToken);

        IEnumerable<KeyValuePair<ExpressionResult, ExpressionResult>> pairs;
        if (iterableResult.ResultType is ResultType.List)
            pairs = iterableResult.ExpectList().Value.Select((value, index) => new KeyValuePair<ExpressionResult, ExpressionResult>(new NumberResult(index), value));
        else if (iterableResult.ResultType is ResultType.Map)
            pairs = iterableResult.ExpectMap().Value.Select(kvp => new KeyValuePair<ExpressionResult, ExpressionResult>(new StringResult(kvp.Key), kvp.Value));
        else
            throw new InvalidResultTypeException(iterableResult, ResultType.List, ResultType.Map);

        foreach (var pair in pairs)
        {
            interpreter.PushScope();

            if (keyToken != null)
            {
                try
                {
                    interpreter.CurrentScope.UpdateValue(keyToken, pair.Key);
                }
                catch (IncompatibleVariableTypeException e)
                {
                    throw new InvalidVariableAssignmentException(keyToken, e);
                }
            }

            try {
                interpreter.CurrentScope.UpdateValue(valueToken, pair.Value);
            }
            catch (IncompatibleVariableTypeException e)
            {
                throw new InvalidVariableAssignmentException(valueToken, e);
            }

            await interpreter.InterpretAsync(statements.ToAsyncEnumerable(), cancellationToken);

            interpreter.PopScope();

            if (interpreter.BreakDepth <= 0)
                continue;

            interpreter.BreakDepth--;

            break;
        }
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugRenderContent()
    {
        return $"{keyVariableExpression?.ToString() ?? "<void>"}: {valueVariableExpression} in {iterableExpression} {{ [{statements.Count} statements] }}";
    }
}