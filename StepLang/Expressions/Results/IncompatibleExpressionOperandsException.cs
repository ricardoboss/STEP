using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Expressions.Results;

/// <summary>
/// Thrown when an expression is given operands that are not compatible with the used operator.
/// </summary>
public class IncompatibleExpressionOperandsException : IncompatibleTypesException
{
    /// <summary>
    /// Creates a new <see cref="IncompatibleExpressionOperandsException"/> with the given location, operand and operation.
    /// </summary>
    /// <param name="location">The location of the expression.</param>
    /// <param name="a">The operand.</param>
    /// <param name="operation">The operation.</param>
    public IncompatibleExpressionOperandsException(TokenLocation location, ExpressionResult a, string operation) : this(location, $"Cannot use the {operation} on values of type {a.ResultType.ToTypeName()}", "Make sure the operand has a type that is compatible with the operator you are trying to use.") { }

    /// <summary>
    /// Creates a new <see cref="IncompatibleExpressionOperandsException"/> with the given location, operands and operation.
    /// </summary>
    /// <param name="location">The location of the expression.</param>
    /// <param name="a">The first operand.</param>
    /// <param name="b">The second operand.</param>
    /// <param name="operation">The operation.</param>
    public IncompatibleExpressionOperandsException(TokenLocation location, ExpressionResult a, ExpressionResult b, string operation) : this(location, $"Cannot use the {operation} operator on values of type {a.ResultType.ToTypeName()} and {b.ResultType.ToTypeName()}", "Make sure the operands are of the same type or check if the used operator can be used on the given types.")
    {
    }

    private IncompatibleExpressionOperandsException(TokenLocation location, string message, string hint) : base(2, location, message, hint) { }
}