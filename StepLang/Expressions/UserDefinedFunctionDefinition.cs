using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Expressions;

public class UserDefinedFunctionDefinition : FunctionDefinition
{
    private readonly IReadOnlyList<IVariableDeclarationNode> parameters;
    private readonly IReadOnlyList<StatementNode> body;

    public UserDefinedFunctionDefinition(IReadOnlyList<IVariableDeclarationNode> parameters, IReadOnlyList<StatementNode> body)
    {
        this.parameters = parameters;
        this.body = body;
    }

    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        if (arguments.Count != parameters.Count)
            throw new InvalidArgumentCountException(parameters.Count, arguments.Count);

        // evaluate args before pushing scope
        var evaldArgs = arguments.Select(a => a.EvaluateUsing(interpreter)).ToList();

        interpreter.PushScope();

        try
        {
            // create the parameter variables in the new scope
            EvaluateParameters(interpreter, evaldArgs);
        }
        catch (NonNullableVariableAssignmentException e)
        {
            throw new InvalidArgumentTypeException(null, e.Variable.Types, e.NewValue);
        }

        interpreter.Execute(body.ToList());

        return interpreter.PopScope().TryGetResult(out var result) ? result : VoidResult.Instance;
    }

    public override IReadOnlyCollection<IVariableDeclarationNode> Parameters => parameters;

    // TODO: implement return type declarations on user defined functions
    public override IEnumerable<ResultType> ReturnTypes => Enum.GetValues<ResultType>();

    private void EvaluateParameters(IVariableDeclarationEvaluator evaluator, IReadOnlyList<ExpressionResult> arguments)
    {
        for (var i = 0; i < parameters.Count; i++)
        {
            var parameter = parameters[i];
            var argumentValue = arguments[i];

            // this will create the variable in the current scope
            var argument = parameter.EvaluateUsing(evaluator);

            // and set the value
            argument.Assign(argumentValue);
        }
    }

    protected override string DebugBodyString => $"[{body.Count} statements]";
}