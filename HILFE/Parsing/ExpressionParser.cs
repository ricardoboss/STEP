using System.Diagnostics.CodeAnalysis;
using HILFE.Tokenizing;

namespace HILFE.Parsing;

public class ExpressionParser
{
    private readonly Queue<Token> tokenQueue = new();

    public void Add(IEnumerable<Token> tokens)
    {
        foreach (var token in tokens)
            tokenQueue.Enqueue(token);
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
            } else {
                currentGroup.Add(token);
            }
        }

        parser.Add(currentGroup);

        yield return parser.ParseBinaryExpression();
    }

    private Expression ParseBinaryExpression(int parentPrecedence = 0)
    {
        var left = ParsePrimaryExpression();

        while (tokenQueue.Count > 0)
        {
            if (!TryParseOperator(out var token, out var op))
            {
                if (token == null)
                    throw new ParserException("Unexpected end of expression.");

                // FIXME: insert token in front of tokenQueue

                break;
            }

            var precedence = op.Value.Precedence();
            if (precedence >= parentPrecedence)
                continue;

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
            case TokenType.Identifier when tokenQueue.Count == 0 || PeekType() != TokenType.ExpressionOpener:
                return new Expression.VariableExpression(currentToken);
            case TokenType.Identifier:
                Match(TokenType.ExpressionOpener);

                var innerExpressionTokens = tokenQueue
                    .Reverse()
                    .SkipWhile(t => t.Type != TokenType.ExpressionCloser)
                    .Reverse()
                    .ToList();
                var expressions = ParseExpressions(innerExpressionTokens).ToList();

                Match(TokenType.ExpressionCloser);

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

                Match(TokenType.ExpressionCloser);

                return expression;
            default:
                throw new InvalidOperationException($"Invalid expression. Got token: {currentToken}");
        }
    }

    private bool TryParseOperator(out Token? currentToken, [NotNullWhen(true)] out BinaryExpressionOperator? op)
    {
        op = null;
        if (!tokenQueue.TryDequeue(out currentToken))
            return false;

        var currentTokenType = currentToken.Type;

        switch (currentTokenType)
        {
            case TokenType.PlusSymbol:
                op = BinaryExpressionOperator.Plus;
                return true;
            case TokenType.MinusSymbol:
                op = BinaryExpressionOperator.Minus;
                return true;
            case TokenType.AsteriskSymbol:
                op = BinaryExpressionOperator.Multiply;
                return true;
            case TokenType.SlashSymbol:
                op = BinaryExpressionOperator.Divide;
                return true;
            case TokenType.PercentSymbol:
                op = BinaryExpressionOperator.Modulo;
                return true;
            case TokenType.EqualsSymbol:
                Match(TokenType.EqualsSymbol);

                op = BinaryExpressionOperator.Equal;
                return true;
            case TokenType.ExclamationMarkSymbol:
                Match(TokenType.EqualsSymbol);

                op = BinaryExpressionOperator.NotEqual;
                return true;
            case TokenType.GreaterThanSymbol when PeekType() is TokenType.EqualsSymbol:
                op = BinaryExpressionOperator.GreaterThanOrEqual;
                return true;
            case TokenType.GreaterThanSymbol:
                op = BinaryExpressionOperator.GreaterThan;
                return true;
            case TokenType.LessThanSymbol when PeekType() is TokenType.EqualsSymbol:
                op = BinaryExpressionOperator.LessThanOrEqual;
                return true;
            case TokenType.LessThanSymbol:
                op = BinaryExpressionOperator.LessThan;
                return true;
            case TokenType.AmpersandSymbol:
                Match(TokenType.AmpersandSymbol);

                op = BinaryExpressionOperator.LogicalAnd;
                return true;
            case TokenType.PipeSymbol:
                Match(TokenType.PipeSymbol);

                op = BinaryExpressionOperator.LogicalOr;
                return true;
            case TokenType.QuestionMarkSymbol:
                Match(TokenType.QuestionMarkSymbol);

                op = BinaryExpressionOperator.Coalesce;
                return true;
            default:
                return false;
        }
    }

    private TokenType? PeekType()
    {
        if (!tokenQueue.TryPeek(out var currentToken))
            return null;

        return currentToken.Type;
    }

    private void Match(TokenType expectedType)
    {
        if (!tokenQueue.TryPeek(out var currentToken))
            throw new ParserException("Unexpected end of expression.");

        if (currentToken.Type != expectedType)
            throw new ParserException($"Unexpected token {currentToken}, expected {expectedType}.");
    }
}