namespace HILFE.Parsing;

public enum BinaryExpressionOperator
{
    Plus,
    Minus,
    Multiply,
    Divide,
    Modulo,
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
        const int multiplicative = 1;
        const int additive = 2;
        const int shiftAndRotate = 3;
        const int relational = 4;
        const int equality = 5;
        const int bitwiseAnd = 6;
        const int bitwiseXor = 7;
        const int bitwiseOr = 8;
        const int logicalAnd = 9;
        const int logicalOr = 10;
        const int coalesce = 11;

        return op switch
        {
            BinaryExpressionOperator.Multiply or BinaryExpressionOperator.Divide or BinaryExpressionOperator.Modulo => multiplicative,
            BinaryExpressionOperator.Plus or BinaryExpressionOperator.Minus => additive,
            BinaryExpressionOperator.BitwiseShiftLeft or BinaryExpressionOperator.BitwiseShiftRight or BinaryExpressionOperator.BitwiseRotateLeft or BinaryExpressionOperator.BitwiseRotateRight => shiftAndRotate,
            BinaryExpressionOperator.GreaterThan or BinaryExpressionOperator.LessThan or BinaryExpressionOperator.GreaterThanOrEqual or BinaryExpressionOperator.LessThanOrEqual => relational,
            BinaryExpressionOperator.Equal or BinaryExpressionOperator.NotEqual => equality,
            BinaryExpressionOperator.BitwiseAnd => bitwiseAnd,
            BinaryExpressionOperator.BitwiseOr => bitwiseOr,
            BinaryExpressionOperator.BitwiseXor => bitwiseXor,
            BinaryExpressionOperator.LogicalAnd => logicalAnd,
            BinaryExpressionOperator.LogicalOr => logicalOr,
            BinaryExpressionOperator.Coalesce => coalesce,
            _ => throw new NotImplementedException($"Undefined operator precedence for operator: {op}"),
        };
    }
}