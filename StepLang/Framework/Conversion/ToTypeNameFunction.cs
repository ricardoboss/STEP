using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework.Conversion;

/// <summary>
/// Converts the type of a given variable to a string.
/// </summary>
public class ToTypeNameFunction : NativeFunction
{
    /// <summary>
    /// The identifier of the <see cref="ToTypeNameFunction"/> function.
    /// </summary>
    public const string Identifier = "toTypeName";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(AnyType, "value"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyString;

    /// <inheritdoc />
    public override StringResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        IReadOnlyList<ExpressionNode> arguments)
    {
        CheckArgumentCount(callLocation, arguments);

        var exp = arguments.Single();
        if (exp is not IdentifierExpressionNode varExp)
            throw new InvalidExpressionTypeException(callLocation, "an identifier", exp.GetType().Name);

        var variable = interpreter.CurrentScope.GetVariable(varExp.Identifier);

        return variable.TypeString;
    }
}