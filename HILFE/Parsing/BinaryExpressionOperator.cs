namespace HILFE.Parsing;

public enum BinaryExpressionOperator
{
    Plus,
    Minus,
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
    Index,
}

public static class BinaryExpressionOperatorExtensions
{
    public static int Precedence(this BinaryExpressionOperator op)
    {
        const int index = 13;
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
            BinaryExpressionOperator.Index => index,
            BinaryExpressionOperator.Power => power,
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