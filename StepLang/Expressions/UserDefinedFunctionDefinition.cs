using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

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

    protected override string DebugBodyString => $"[{body.Count} statements]";

    public override IReadOnlyList<IVariableDeclarationNode> Parameters => parameters;

    public IReadOnlyList<StatementNode> Body => body;

    // TODO: implement return type declarations on user defined functions
    protected override IEnumerable<ResultType> ReturnTypes => Enum.GetValues<ResultType>();

    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        // TODO: this needs to be adjusted for default parameter values
        if (arguments.Count != parameters.Count)
            throw new InvalidArgumentCountException(parameters.Count, arguments.Count);

        // evaluate args before pushing scope
        var evaldArgs = arguments.Select(a => (a.Location, a.EvaluateUsing(interpreter))).ToList();

        interpreter.PushScope();

        // create the parameter variables in the new scope
        EvaluateParameters(interpreter, evaldArgs);

        interpreter.Execute(body);

        return interpreter.PopScope().TryGetResult(out var result) ? result : VoidResult.Instance;
    }

    private void EvaluateParameters(IVariableDeclarationEvaluator evaluator, IReadOnlyList<(TokenLocation, ExpressionResult)> arguments)
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