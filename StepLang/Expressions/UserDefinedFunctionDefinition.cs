using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

public class UserDefinedFunctionDefinition : FunctionDefinition
{
    private readonly TokenLocation location;
    private readonly IReadOnlyList<IVariableDeclarationNode> parameters;

    public UserDefinedFunctionDefinition(TokenLocation location, IReadOnlyList<IVariableDeclarationNode> parameters, IReadOnlyList<StatementNode> body)
    {
        this.location = location;
        this.parameters = parameters;
        Body = body;
    }

    protected override string DebugBodyString => $"[{Body.Count} statements]";

    public override IReadOnlyList<IVariableDeclarationNode> Parameters => parameters;

    public IReadOnlyList<StatementNode> Body { get; }

    // TODO: implement return type declarations on user defined functions
    protected override IEnumerable<ResultType> ReturnTypes => Enum.GetValues<ResultType>();

    private int RequiredParametersCount => parameters.TakeWhile(n => !n.HasValue).Count();

    private int TotalParametersCount => parameters.Count;

    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        if (arguments.Count < RequiredParametersCount || arguments.Count > TotalParametersCount)
            throw new InvalidArgumentCountException(parameters.Count, arguments.Count);

        // evaluate args _before_ pushing scope
        var evaldArgs = EvaluateArguments(interpreter, arguments);

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

    private List<(TokenLocation, ExpressionResult)> EvaluateArguments(IExpressionEvaluator evaluator, IReadOnlyCollection<ExpressionNode> arguments)
    {
        var evaldArgs = new List<(TokenLocation, ExpressionResult)>(TotalParametersCount);

        foreach (var argExpression in arguments)
        {
            var location = argExpression.Location;
            var value = argExpression.EvaluateUsing(evaluator);

            evaldArgs.Add((location, value));
        }

        for (var i = arguments.Count; i < TotalParametersCount; i++)
        {
            var parameter = parameters[i];
            var location = parameter.Location;
            var value = parameter switch
            {
                NullableVariableInitializationNode nullable => nullable.Expression.EvaluateUsing(evaluator),
                VariableInitializationNode initialization => initialization.Expression.EvaluateUsing(evaluator),
                _ => throw new InvalidOperationException("Invalid parameter type"),
            };

            evaldArgs.Add((location, value));
        }

        return evaldArgs;
    }

    private void CreateArgumentVariables(IVariableDeclarationEvaluator evaluator, IReadOnlyList<(TokenLocation, ExpressionResult)> arguments)
    {
        for (var i = 0; i < parameters.Count; i++)
        {
            var parameter = parameters[i];
            var (argumentLocation, argumentValue) = arguments[i];

            // this will create the variable in the current scope
            var argument = parameter.EvaluateUsing(evaluator);

            // and set the value
            argument.Assign(argumentLocation, argumentValue);
        }
    }
}