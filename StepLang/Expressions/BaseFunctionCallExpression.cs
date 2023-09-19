using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

public abstract class BaseFunctionCallExpression : Expression
{
    protected IReadOnlyList<Expression> Args { get; }

    protected BaseFunctionCallExpression(IReadOnlyList<Expression> args)
    {
        Args = args;
    }

    protected abstract TokenLocation? GetCallLocation();

    protected async Task<ExpressionResult> ExecuteFunction(FunctionDefinition function, Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        try
        {
            return await function.EvaluateAsync(interpreter, Args, cancellationToken);
        }
        catch (InvalidArgumentCountException e)
        {
            throw new InvalidFunctionCallException(GetCallLocation(), e);
        }
        catch (InvalidArgumentTypeException e)
        {
            throw new InvalidFunctionCallException(GetCallLocation(), e);
        }
    }
}