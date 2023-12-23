namespace StepLang.Parsing;

/// <summary>
/// All binary operators that can be used in expressions.
/// </summary>
public enum BinaryExpressionOperator
{
    /// <summary>
    /// Represents the addition operator (<c>+</c>)
    /// </summary>
    Add,

    /// <summary>
    /// Represents the subtraction operator (<c>-</c>)
    /// </summary>
    Subtract,

    /// <summary>
    /// Represents the multiplication operator (<c>*</c>)
    /// </summary>
    Multiply,

    /// <summary>
    /// Represents the division operator (<c>/</c>)
    /// </summary>
    Divide,

    /// <summary>
    /// Represents the modulo operator (<c>%</c>)
    /// </summary>
    Modulo,

    /// <summary>
    /// Represents the power operator (<c>**</c>)
    /// </summary>
    Power,

    /// <summary>
    /// Represents the greater than operator (<c>&gt;</c>)
    /// </summary>
    GreaterThan,

    /// <summary>
    /// Represents the less than operator (<c>&lt;</c>)
    /// </summary>
    LessThan,

    /// <summary>
    /// Represents the greater than or equal operator (<c>&gt;=</c>)
    /// </summary>
    GreaterThanOrEqual,

    /// <summary>
    /// Represents the less than or equal operator (<c>&lt;=</c>)
    /// </summary>
    LessThanOrEqual,

    /// <summary>
    /// Represents the equality operator (<c>==</c>)
    /// </summary>
    Equal,

    /// <summary>
    /// Represents the inequality operator (<c>!=</c>)
    /// </summary>
    NotEqual,

    /// <summary>
    /// Represents the logical and operator (<c>&amp;&amp;</c>)
    /// </summary>
    LogicalAnd,

    /// <summary>
    /// Represents the logical or operator (<c>||</c>)
    /// </summary>
    LogicalOr,

    /// <summary>
    /// Represents the bitwise and operator (<c>&amp;</c>)
    /// </summary>
    BitwiseAnd,

    /// <summary>
    /// Represents the bitwise or operator (<c>|</c>)
    /// </summary>
    BitwiseOr,

    /// <summary>
    /// Represents the bitwise xor operator (<c>^</c>)
    /// </summary>
    BitwiseXor,

    /// <summary>
    /// Represents the bitwise shift left operator (<c>&lt;&lt;</c>)
    /// </summary>
    BitwiseShiftLeft,

    /// <summary>
    /// Represents the bitwise shift right operator (<c>&gt;&gt;</c>)
    /// </summary>
    BitwiseShiftRight,

    /// <summary>
    /// Represents the bitwise rotate left operator (<c>&lt;&lt;&lt;</c>)
    /// </summary>
    BitwiseRotateLeft,

    /// <summary>
    /// Represents the bitwise rotate right operator (<c>&gt;&gt;&gt;</c>)
    /// </summary>
    BitwiseRotateRight,

    /// <summary>
    /// Represents the null coalescing operator (<c>??</c>)
    /// </summary>
    Coalesce,
}

/// <summary>
/// Extension methods for <see cref="BinaryExpressionOperator"/>.
/// </summary>
public static class BinaryExpressionOperatorExtensions
{
    /// <summary>
    /// Gets the precedence of the given operator.
    /// </summary>
    /// <param name="op">The operator.</param>
    /// <returns>An integer representing the precedence of the operator. Comparing precedences of two operators tells you which one should be evaluated first, with a higher precedence being evaluated first.</returns>
    /// <exception cref="NotImplementedException">Thrown if the operators precendence is not defined.</exception>
    public static int Precedence(this BinaryExpressionOperator op)
    {
        // these consts must be at most one apart
        const int power = 12;
        const int multiplicative = 11;
        const int additive = 10;
        const int shiftAndRotate = 9;
        const int relational = 8;
        const int equality = 7;
        const int bitwiseAnd = 6;
        const int bitwiseXor = 5;
        const int bitwiseOr = 4;
        const int logicalAnd = 3;
        const int logicalOr = 2;
        const int coalesce = 1;

        return op switch
        {
            BinaryExpressionOperator.Power => power,
            BinaryExpressionOperator.Multiply or BinaryExpressionOperator.Divide or BinaryExpressionOperator.Modulo => multiplicative,
            BinaryExpressionOperator.Add or BinaryExpressionOperator.Subtract => additive,
            BinaryExpressionOperator.BitwiseShiftLeft or BinaryExpressionOperator.BitwiseShiftRight or BinaryExpressionOperator.BitwiseRotateLeft or BinaryExpressionOperator.BitwiseRotateRight => shiftAndRotate,
            BinaryExpressionOperator.GreaterThan or BinaryExpressionOperator.LessThan or BinaryExpressionOperator.GreaterThanOrEqual or BinaryExpressionOperator.LessThanOrEqual => relational,
            BinaryExpressionOperator.Equal or BinaryExpressionOperator.NotEqual => equality,
            BinaryExpressionOperator.BitwiseAnd => bitwiseAnd,
            BinaryExpressionOperator.BitwiseOr => bitwiseOr,
            BinaryExpressionOperator.BitwiseXor => bitwiseXor,
            BinaryExpressionOperator.LogicalAnd => logicalAnd,
            BinaryExpressionOperator.LogicalOr => logicalOr,
            BinaryExpressionOperator.Coalesce => coalesce,
            _ => throw new NotSupportedException($"Undefined operator precedence for operator: {op}"),
        };
    }

    /// <summary>
    /// Gets the symbol of the given operator.
    /// </summary>
    /// <param name="op">The operator.</param>
    /// <returns>A string representing the symbol of the operator.</returns>
    /// <exception cref="NotImplementedException">Thrown if the operators symbol is not defined.</exception>
    public static string ToSymbol(this BinaryExpressionOperator op)
    {
        return op switch
        {
            BinaryExpressionOperator.Add => "+",
            BinaryExpressionOperator.Subtract => "-",
            BinaryExpressionOperator.Multiply => "*",
            BinaryExpressionOperator.Divide => "/",
            BinaryExpressionOperator.Modulo => "%",
            BinaryExpressionOperator.Power => "**",
            BinaryExpressionOperator.GreaterThan => ">",
            BinaryExpressionOperator.LessThan => "<",
            BinaryExpressionOperator.GreaterThanOrEqual => ">=",
            BinaryExpressionOperator.LessThanOrEqual => "<=",
            BinaryExpressionOperator.Equal => "==",
            BinaryExpressionOperator.NotEqual => "!=",
            BinaryExpressionOperator.LogicalAnd => "&&",
            BinaryExpressionOperator.LogicalOr => "||",
            BinaryExpressionOperator.BitwiseAnd => "&",
            BinaryExpressionOperator.BitwiseOr => "|",
            BinaryExpressionOperator.BitwiseXor => "^",
            BinaryExpressionOperator.BitwiseShiftLeft => "<<",
            BinaryExpressionOperator.BitwiseShiftRight => ">>",
            BinaryExpressionOperator.BitwiseRotateLeft => "<<<",
            BinaryExpressionOperator.BitwiseRotateRight => ">>>",
            BinaryExpressionOperator.Coalesce => "??",
            _ => throw new NotSupportedException($"Undefined operator symbol for operator: {op}"),
        };
    }
}