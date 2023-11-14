namespace StepLang.Parsing;

public enum BinaryExpressionOperator
{
    Add,
    Subtract,
    Multiply,
    Divide,
    Modulo,
    Power,
    GreaterThan,
    LessThan,
    GreaterThanOrEqual,
    LessThanOrEqual,
    Equal,
    NotEqual,
    LogicalAnd,
    LogicalOr,
    BitwiseAnd,
    BitwiseOr,
    BitwiseXor,
    BitwiseShiftLeft,
    BitwiseShiftRight,
    BitwiseRotateLeft,
    BitwiseRotateRight,
    Coalesce,
}

public static class BinaryExpressionOperatorExtensions
{
    public static int Precedence(this BinaryExpressionOperator op)
    {
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