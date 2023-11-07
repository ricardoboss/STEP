using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Conversion;

public class ToTypeNameFunction : NativeFunction
{
    public const string Identifier = "toTypeName";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(AnyType, "value"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyString;

    /// <inheritdoc />
    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        CheckArgumentCount(arguments);

        var exp = arguments.Single();
        if (exp is not IdentifierExpressionNode varExp)
            throw new InvalidExpressionTypeException("an identifier", exp.GetType().Name);

        var variable = interpreter.CurrentScope.GetVariable(varExp.Identifier);

        return new StringResult(variable.TypeString);
    }
}