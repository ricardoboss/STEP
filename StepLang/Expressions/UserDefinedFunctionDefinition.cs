using System.Diagnostics;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

/// <summary>
/// A function definition that is defined by the user.
/// </summary>
public class UserDefinedFunctionDefinition : FunctionDefinition
{
    private readonly TokenLocation location;
    private readonly IReadOnlyList<IVariableDeclarationNode> parameters;

    /// <summary>
    /// Creates a new user-defined function definition.
    /// </summary>
    /// <param name="location">The location of the function definition.</param>
    /// <param name="parameters">The parameters of the function.</param>
    /// <param name="body">The body of the function.</param>
    public UserDefinedFunctionDefinition(TokenLocation location, IReadOnlyList<IVariableDeclarationNode> parameters, IReadOnlyList<StatementNode> body)
    {
        this.location = location;
        this.parameters = parameters;
        Body = body;
    }

    /// <inheritdoc />
    protected override string DebugBodyString => $"[{Body.Count} statements]";

    /// <inheritdoc />
    public override IReadOnlyList<IVariableDeclarationNode> Parameters => parameters;

    /// <summary>
    /// The statements of the user defined function.
    /// </summary>
    public IReadOnlyList<StatementNode> Body { get; }

    // TODO: implement return type declarations on user defined functions
    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes => Enum.GetValues<ResultType>();

    private int RequiredParametersCount => parameters.TakeWhile(n => !n.HasValue).Count();

    private int TotalParametersCount => parameters.Count;

    /// <inheritdoc />
    public override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        if (arguments.Count < RequiredParametersCount || arguments.Count > TotalParametersCount)
            throw new InvalidArgumentCountException(callLocation, parameters.Count, arguments.Count);

        // evaluate args _before_ pushing scope
        var evaldArgs = EvaluateArguments(interpreter, arguments).ToList();

        interpreter.PushScope();

        // create the parameter variables in the new scope
        CreateArgumentVariables(interpreter, evaldArgs);

        interpreter.Execute(Body);

        if (interpreter.PopScope().TryGetResult(out var resultValue, out var resultLocation))
            return CheckResult(resultLocation, resultValue);

        return CheckResult(location, VoidResult.Instance);
    }

    private ExpressionResult CheckResult(TokenLocation resultLocation, ExpressionResult resultValue)
    {
        if (!ReturnTypes.Contains(resultValue.ResultType))
            throw new InvalidReturnTypeException(resultLocation, resultValue.ResultType, ReturnTypes);

        return resultValue;
    }

    private IEnumerable<(TokenLocation, ExpressionResult)> EvaluateArguments(IExpressionEvaluator evaluator, IEnumerable<ExpressionNode> arguments)
    {
        var suppliedArgs = 0;
        foreach (var argExpression in arguments)
        {
            var argLocation = argExpression.Location;
            var argValue = argExpression.EvaluateUsing(evaluator);

            suppliedArgs++;

            yield return (argLocation, argValue);
        }

        Debug.Assert(suppliedArgs <= TotalParametersCount);

        for (; suppliedArgs < TotalParametersCount; suppliedArgs++)
        {
            var parameter = parameters[suppliedArgs];
            var parameterLocation = parameter.Location;
            var defaultValue = parameter switch
            {
                NullableVariableInitializationNode nullable => nullable.Expression.EvaluateUsing(evaluator),
                VariableInitializationNode initialization => initialization.Expression.EvaluateUsing(evaluator),
                _ => throw new InvalidOperationException("Invalid parameter type"),
            };

            yield return (parameterLocation, defaultValue);
        }

        Debug.Assert(suppliedArgs == TotalParametersCount);
    }

    private void CreateArgumentVariables(IVariableDeclarationEvaluator evaluator, IReadOnlyList<(TokenLocation, ExpressionResult)> arguments)
    {
        for (var i = 0; i < parameters.Count; i++)
        {
            var parameter = parameters[i];
            var (assignmentLocation, argumentValue) = arguments[i];

            // this will create the variable in the current scope
            var argument = parameter.EvaluateUsing(evaluator);

            // and set the value
            argument.Assign(assignmentLocation, argumentValue);
        }
    }
}