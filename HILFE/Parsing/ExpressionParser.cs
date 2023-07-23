using System.Diagnostics.CodeAnalysis;
using HILFE.Tokenizing;

namespace HILFE.Parsing;

public class ExpressionParser
{
    private readonly TokenQueue tokenQueue = new();

    public void Add(IEnumerable<Token> tokens)
    {
        tokenQueue.Enqueue(tokens);
    }

    public static Expression Parse(IEnumerable<Token> tokens)
    {
        var parser = new ExpressionParser();
        parser.Add(tokens);
        return parser.ParseBinaryExpression();
    }

    public static IEnumerable<Expression> ParseExpressions(IReadOnlyList<Token> tokens)
    {
        if (tokens.Count == 0)
            yield break;

        var parser = new ExpressionParser();
        var currentGroup = new List<Token>();

        foreach (var token in tokens)
        {
            if (token.Type is TokenType.ExpressionSeparator)
            {
                parser.Add(currentGroup);

                yield return parser.ParseBinaryExpression();

                currentGroup = new();
            }
            else
            {
                currentGroup.Add(token);
            }
        }

        parser.Add(currentGroup);

        yield return parser.ParseBinaryExpression();
    }

    private Expression ParseBinaryExpression(int parentPrecedence = 0)
    {
        var left = ParsePrimaryExpression();

        while (tokenQueue.IsNotEmpty)
        {
            if (tokenQueue.PeekType() is TokenType.ExpressionCloser)
                return left;

            if (!TryPeekOperator(out var op, out var opLength))
                throw new ParserException("Unexpected end of expression.");

            var precedence = op.Value.Precedence();
            if (precedence < parentPrecedence)
                break;

            // dequeue operator token
            _ = tokenQueue.Dequeue(opLength.Value);

            var right = ParseBinaryExpression(precedence + 1);

            left = Expression.FromOperator(op.Value, left, right);
        }

        return left;
    }

    private Expression ParsePrimaryExpression()
    {
        var currentToken = tokenQueue.Dequeue();
        var currentTokenType = currentToken.Type;

        switch (currentTokenType)
        {
            case TokenType.Identifier when
                tokenQueue.IsEmpty ||
                tokenQueue.PeekType() != TokenType.ExpressionOpener:
                return new Expression.VariableExpression(currentToken);
            case TokenType.Identifier:
                tokenQueue.Expect(TokenType.ExpressionOpener);

                var innerExpressionTokens = tokenQueue
                    .Consume()
                    .Reverse()
                    .SkipWhile(t => t.Type != TokenType.ExpressionCloser)
                    .Reverse()
                    .ToList();

                var expressions = ParseExpressions(innerExpressionTokens).ToList();

                tokenQueue.Expect(TokenType.ExpressionCloser);

                return new Expression.FunctionCallExpression(currentToken, expressions);
            case TokenType.LiteralNumber when double.TryParse(currentToken.Value, out var value):
                return Expression.Constant(value);
            case TokenType.LiteralNumber:
                throw new FormatException("Invalid number format.");
            case TokenType.LiteralBoolean when bool.TryParse(currentToken.Value, out var value):
                return Expression.Constant(value);
            case TokenType.LiteralBoolean:
                throw new FormatException($"Invalid bool format: {currentToken.Value}");
            case TokenType.LiteralString:
                return Expression.Constant(currentToken.Value);
            case TokenType.ExpressionOpener:
                var expression = ParseBinaryExpression();

                tokenQueue.Expect(TokenType.ExpressionCloser);

                return expression;
            default:
                throw new InvalidOperationException($"Invalid expression. Got token: {currentToken}");
        }
    }

    private bool TryPeekOperator([NotNullWhen(true)] out BinaryExpressionOperator? op, [NotNullWhen(true)] out int? opLength)
    {
        op = null;
        opLength = null;
        if (!tokenQueue.TryPeekType(out var currentTokenType))
            return false;

        switch (currentTokenType)
        {
            case TokenType.PlusSymbol:
                op = BinaryExpressionOperator.Plus;
                opLength = 1;
                return true;
            case TokenType.MinusSymbol:
                op = BinaryExpressionOperator.Minus;
                opLength = 1;
                return true;
            case TokenType.AsteriskSymbol:
                op = BinaryExpressionOperator.Multiply;
                opLength = 1;
                return true;
            case TokenType.SlashSymbol:
                op = BinaryExpressionOperator.Divide;
                opLength = 1;
                return true;
            case TokenType.PercentSymbol:
                op = BinaryExpressionOperator.Modulo;
                opLength = 1;
                return true;
            case TokenType.EqualsSymbol when tokenQueue.PeekType(1) is TokenType.EqualsSymbol:
                op = BinaryExpressionOperator.Equal;
                opLength = 2;
                return true;
            case TokenType.ExclamationMarkSymbol when tokenQueue.PeekType(1) is TokenType.EqualsSymbol:
                op = BinaryExpressionOperator.NotEqual;
                opLength = 2;
                return true;
            case TokenType.GreaterThanSymbol when tokenQueue.PeekType(1) is TokenType.EqualsSymbol:
                op = BinaryExpressionOperator.GreaterThanOrEqual;
                opLength = 2;
                return true;
            case TokenType.GreaterThanSymbol:
                op = BinaryExpressionOperator.GreaterThan;
                opLength = 1;
                return true;
            case TokenType.LessThanSymbol when tokenQueue.PeekType(1) is TokenType.EqualsSymbol:
                op = BinaryExpressionOperator.LessThanOrEqual;
                opLength = 2;
                return true;
            case TokenType.LessThanSymbol:
                op = BinaryExpressionOperator.LessThan;
                opLength = 1;
                return true;
            case TokenType.AmpersandSymbol when tokenQueue.PeekType(1) is TokenType.AmpersandSymbol:
                op = BinaryExpressionOperator.LogicalAnd;
                opLength = 2;
                return true;
            case TokenType.PipeSymbol when tokenQueue.PeekType(1) is TokenType.PipeSymbol:
                op = BinaryExpressionOperator.LogicalOr;
                opLength = 2;
                return true;
            case TokenType.QuestionMarkSymbol when tokenQueue.PeekType(1) is TokenType.QuestionMarkSymbol:
                op = BinaryExpressionOperator.Coalesce;
                opLength = 2;
                return true;
            default:
                throw new ParserException($"Unexpected operator: {tokenQueue.Peek()}");
        }
    }
}