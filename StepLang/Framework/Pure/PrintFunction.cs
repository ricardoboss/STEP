using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Pure;

public class PrintFunction : NativeFunction
{
    public const string Identifier = "print";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (Enum.GetValues<ResultType>(), "...values") };

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        if (interpreter.StdOut is not { } stdOut)
            return VoidResult.Instance;

        FunctionDefinition toStringFunction;
        if (interpreter.CurrentScope.TryGetVariable(ToStringFunction.Identifier, out var toStringVar))
            toStringFunction = toStringVar.Value.ExpectFunction().Value;
        else
            toStringFunction = new ToStringFunction();

        var stringArgs = await arguments
            .EvaluateAsync(interpreter, cancellationToken)
            .SelectAwait(async exp => await toStringFunction.EvaluateAsync(interpreter, new[] { new ConstantExpression(exp) }, cancellationToken))
            .Select(r => r.ExpectString().Value)
            .ToListAsync(cancellationToken);

        await Print(stdOut, string.Join("", stringArgs), cancellationToken);

        return VoidResult.Instance;
    }

    protected virtual async Task Print(TextWriter output, string value, CancellationToken cancellationToken = default)
        => await output.WriteAsync(value.AsMemory(), cancellationToken);
}