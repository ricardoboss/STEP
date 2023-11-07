using System.Globalization;
using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record LiteralExpressionNode(Token Literal) : ExpressionNode
{
    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }

    public override TokenLocation Location => Literal.Location;

    public static implicit operator LiteralExpressionNode(string literal) => new(new Token(TokenType.LiteralString, literal));

    public static implicit operator LiteralExpressionNode(double literal) => new(new Token(TokenType.LiteralNumber, literal.ToString(CultureInfo.InvariantCulture)));

    public static implicit operator LiteralExpressionNode(int literal) => new(new Token(TokenType.LiteralNumber, literal.ToString(CultureInfo.InvariantCulture)));

    public static implicit operator LiteralExpressionNode(bool literal) => new(new Token(TokenType.LiteralBoolean, literal ? "true" : "false"));

    public static LiteralExpressionNode FromString(string value) => value;
    public static LiteralExpressionNode FromDouble(double value) => value;
    public static LiteralExpressionNode FromInt32(int value) => value;
    public static LiteralExpressionNode FromBoolean(bool value) => value;
}