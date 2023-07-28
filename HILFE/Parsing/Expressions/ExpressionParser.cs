using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using HILFE.Parsing.Statements;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Expressions;

public class ExpressionParser
{
    private readonly TokenQueue tokenQueue = new();

    public void Add(IEnumerable<Token> tokens)
    {
        tokenQueue.Enqueue(tokens);
    }

    public static async Task<Expression> ParseAsync(IEnumerable<Token> tokens, CancellationToken cancellationToken = default)
    {
        var parser = new ExpressionParser();
        parser.Add(tokens);
        return await parser.ParseBinaryExpression(cancellationToken: cancellationToken);
    }

    public static async IAsyncEnumerable<Expression> ParseExpressionsAsync(IReadOnlyList<Token> tokens, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (tokens.Count == 0)
            yield break;

        var parser = new ExpressionParser();
        var currentGroup = new List<Token>();
        var expressionDepth = 0;
        var codeBlockDepth = 0;
        var listDepth = 0;

        foreach (var token in tokens)
        {
            if (token.Type is TokenType.CommaSymbol && expressionDepth == 0 && codeBlockDepth == 0 && listDepth == 0)
            {
                parser.Add(currentGroup);

                yield return await parser.ParseBinaryExpression(cancellationToken: cancellationToken);

                currentGroup = new();
            }
            else
            {
                currentGroup.Add(token);

                switch (token.Type)
                {
                    case TokenType.OpeningParentheses:
                        expressionDepth++;
                        break;
                    case TokenType.ClosingParentheses:
                        expressionDepth--;
                        break;
                    case TokenType.OpeningCurlyBracket:
                        codeBlockDepth++;
                        break;
                    case TokenType.ClosingCurlyBracket:
                        codeBlockDepth--;
                        break;
                    case TokenType.OpeningSquareBracket:
                        listDepth++;
                        break;
                    case TokenType.ClosingSquareBracket:
                        listDepth--;
                        break;
                }
            }
        }

        parser.Add(currentGroup);

        yield return await parser.ParseBinaryExpression(cancellationToken: cancellationToken);
    }

    private static async IAsyncEnumerable<KeyValuePair<string, Expression>> ParseMapTokens(IReadOnlyCollection<Token> tokens, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (tokens.Count == 0)
            yield break;

        var currentGroup = new TokenQueue();
        var expressionDepth = 0;
        var codeBlockDepth = 0;
        var listDepth = 0;

        foreach (var token in tokens)
        {
            if (token.Type is TokenType.CommaSymbol && expressionDepth == 0 && codeBlockDepth == 0 && listDepth == 0)
            {
                yield return await FinalizeGroup();

                Debug.Assert(currentGroup.IsEmpty);
            }
            else
            {
                currentGroup.Enqueue(token);

                switch (token.Type)
                {
                    case TokenType.OpeningParentheses:
                        expressionDepth++;
                        break;
                    case TokenType.ClosingParentheses:
                        expressionDepth--;
                        break;
                    case TokenType.OpeningCurlyBracket:
                        codeBlockDepth++;
                        break;
                    case TokenType.ClosingCurlyBracket:
                        codeBlockDepth--;
                        break;
                    case TokenType.OpeningSquareBracket:
                        listDepth++;
                        break;
                    case TokenType.ClosingSquareBracket:
                        listDepth--;
                        break;
                }
            }
        }

        yield return await FinalizeGroup();

        yield break;

        async Task<KeyValuePair<string, Expression>> FinalizeGroup()
        {
            var keyTokens = currentGroup.DequeueUntil(TokenType.ColonSymbol);
            _ = currentGroup.Dequeue(TokenType.ColonSymbol);
            var valueTokens = currentGroup.DequeueUntil(TokenType.CommaSymbol);

            var keyExpression = await ParseAsync(keyTokens, cancellationToken);

            if (keyExpression is not ConstantExpression { Value: string key })
                throw new UnexpectedTokenException(keyTokens[0], TokenType.LiteralString);

            var valueExpression = await ParseAsync(valueTokens, cancellationToken);

            return new(key, valueExpression);
        }
    }

    private async Task<Expression> ParseBinaryExpression(int parentPrecedence = 0, CancellationToken cancellationToken = default)
    {
        var left = await ParseUnaryExpression(cancellationToken);

        while (tokenQueue.IsNotEmpty)
        {
            if (tokenQueue.PeekType() is TokenType.ClosingParentheses or TokenType.ClosingSquareBracket)
                return left;

            if (!TryPeekOperator(out var op, out var opPreLength, out var opPostLength))
                throw new ParserException("Unexpected end of expression.");

            var precedence = op.Value.Precedence();
            if (precedence < parentPrecedence)
                break;

            // dequeue operator tokens before right value
            _ = tokenQueue.Dequeue(opPreLength.Value);

            var right = await ParseBinaryExpression(precedence + 1, cancellationToken);

            // dequeue operator tokens after right value
            _ = tokenQueue.Dequeue(opPostLength.Value);

            left = Expression.FromOperator(op.Value, left, right);
        }

        return left;
    }

    private async Task<Expression> ParseUnaryExpression(CancellationToken cancellationToken = default)
    {
        var currentToken = tokenQueue.Dequeue();
        var currentTokenType = currentToken.Type;

        switch (currentTokenType)
        {
            case TokenType.Identifier when
                tokenQueue.IsEmpty ||
                tokenQueue.PeekType() != TokenType.OpeningParentheses:
                return new VariableExpression(currentToken);
            case TokenType.Identifier:
                tokenQueue.Dequeue(TokenType.OpeningParentheses);

                var innerExpressionTokens = tokenQueue.DequeueUntil(TokenType.ClosingParentheses);

                tokenQueue.Dequeue(TokenType.ClosingParentheses);

                var expressions = await ParseExpressionsAsync(innerExpressionTokens, cancellationToken).ToListAsync(cancellationToken);

                return new IdentifierFunctionCallExpression(currentToken, expressions);
            case TokenType.LiteralNumber when double.TryParse(currentToken.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var value):
                return Expression.Constant(value);
            case TokenType.LiteralNumber:
                throw new FormatException("Invalid number format.");
            case TokenType.LiteralBoolean when bool.TryParse(currentToken.Value, out var value):
                return Expression.Constant(value);
            case TokenType.LiteralBoolean:
                throw new FormatException($"Invalid bool format: {currentToken.Value}");
            case TokenType.LiteralString:
                return Expression.Constant(currentToken.Value);
            case TokenType.OpeningParentheses:
                var nextType = tokenQueue.PeekType();
                if (nextType is TokenType.ClosingParentheses)
                {
                    if (tokenQueue.PeekType(1) is not TokenType.OpeningCurlyBracket)
                        throw new ParserException("Empty pair of parentheses is not a valid expression.");

                    // function declaration

                    tokenQueue.Prepend(currentToken);

                    return await ParseFunctionDefinitionExpression(cancellationToken);
                }

                if (nextType is TokenType.TypeName)
                {
                    // function declaration

                    tokenQueue.Prepend(currentToken);

                    return await ParseFunctionDefinitionExpression(cancellationToken);
                }

                // parentheses around expression

                var innerExpression = await ParseBinaryExpression(cancellationToken: cancellationToken);

                tokenQueue.Dequeue(TokenType.ClosingParentheses);

                return innerExpression;
            case TokenType.OpeningSquareBracket:
                var listExpressionTokens = tokenQueue.DequeueUntil(TokenType.ClosingSquareBracket);

                tokenQueue.Dequeue(TokenType.ClosingSquareBracket);

                var listExpressions = await ParseExpressionsAsync(listExpressionTokens, cancellationToken).ToListAsync(cancellationToken);

                return new ListExpression(listExpressions);
            case TokenType.OpeningCurlyBracket:
                var mapExpressionTokens = tokenQueue.DequeueUntil(TokenType.ClosingCurlyBracket);

                tokenQueue.Dequeue(TokenType.ClosingCurlyBracket);

                var mapExpressions = await ParseMapTokens(mapExpressionTokens, cancellationToken).ToListAsync(cancellationToken);
                var map = mapExpressions.ToDictionary(p => p.Key, p => p.Value);

                return new MapExpression(map);
            case var _ when currentTokenType.IsMathematicalOperation():
                throw new NotImplementedException("Cannot parse mathematical operation as expression");
            default:
                throw new InvalidOperationException($"Invalid expression. Got token: {currentToken}");
        }
    }

    private async Task<Expression> ParseFunctionDefinitionExpression(CancellationToken cancellationToken = default)
    {
        var parameters = new List<(Token, Token)>(); // (type, name)

        tokenQueue.Dequeue(TokenType.OpeningParentheses);

        while (tokenQueue.TryPeekType(out var tokenType) && tokenType is TokenType.TypeName)
        {
            var type = tokenQueue.Dequeue(TokenType.TypeName);
            var name = tokenQueue.Dequeue(TokenType.Identifier);
            
            parameters.Add((type, name));

            if (tokenQueue.TryPeekType(out var nextTokenType) && nextTokenType is TokenType.CommaSymbol)
                _ = tokenQueue.Dequeue();
        }

        tokenQueue.Dequeue(TokenType.ClosingParentheses);
        tokenQueue.Dequeue(TokenType.OpeningCurlyBracket);
        
        var statementParser = new StatementParser();
        var codeBlockDepth = 0;
        while (tokenQueue.PeekType() is not TokenType.ClosingCurlyBracket || codeBlockDepth > 0)
        {
            var token = tokenQueue.Dequeue();
            statementParser.Add(token);
            if (token.Type is TokenType.OpeningCurlyBracket)
                codeBlockDepth++;
            else if (token.Type is TokenType.ClosingCurlyBracket)
                codeBlockDepth--;
        }
        var body = await statementParser.ParseAsync(cancellationToken).ToListAsync(cancellationToken);

        tokenQueue.Dequeue(TokenType.ClosingCurlyBracket);

        var definitionExpression = new FunctionDefinitionExpression(parameters, body);

        if (!tokenQueue.TryPeekType(out var nextNextType) || nextNextType is not TokenType.OpeningParentheses)
            return definitionExpression;

        tokenQueue.Dequeue(TokenType.OpeningParentheses);

        var innerExpressionTokens = tokenQueue.DequeueUntil(TokenType.ClosingParentheses);

        tokenQueue.Dequeue(TokenType.ClosingParentheses);

        var args = await ParseExpressionsAsync(innerExpressionTokens, cancellationToken).ToListAsync(cancellationToken);

        return new DirectFunctionDefinitionCallExpression(definitionExpression, args);
    }

    private bool TryPeekOperator([NotNullWhen(true)] out BinaryExpressionOperator? op, [NotNullWhen(true)] out int? opPreLength, [NotNullWhen(true)] out int? opPostLength)
    {
        op = null;
        opPreLength = null;
        opPostLength = null;
        if (!tokenQueue.TryPeekType(out var currentTokenType))
            return false;

        opPreLength = 1;
        opPostLength = 0;
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
            case TokenType.HatSymbol:
                op = BinaryExpressionOperator.Power;
                return true;
            case TokenType.EqualsSymbol when tokenQueue.PeekType(1) is TokenType.EqualsSymbol:
                op = BinaryExpressionOperator.Equal;
                opPreLength = 2;
                return true;
            case TokenType.ExclamationMarkSymbol when tokenQueue.PeekType(1) is TokenType.EqualsSymbol:
                op = BinaryExpressionOperator.NotEqual;
                opPreLength = 2;
                return true;
            case TokenType.GreaterThanSymbol when tokenQueue.PeekType(1) is TokenType.EqualsSymbol:
                op = BinaryExpressionOperator.GreaterThanOrEqual;
                opPreLength = 2;
                return true;
            case TokenType.GreaterThanSymbol:
                op = BinaryExpressionOperator.GreaterThan;
                return true;
            case TokenType.LessThanSymbol when tokenQueue.PeekType(1) is TokenType.EqualsSymbol:
                op = BinaryExpressionOperator.LessThanOrEqual;
                opPreLength = 2;
                return true;
            case TokenType.LessThanSymbol:
                op = BinaryExpressionOperator.LessThan;
                return true;
            case TokenType.AmpersandSymbol when tokenQueue.PeekType(1) is TokenType.AmpersandSymbol:
                op = BinaryExpressionOperator.LogicalAnd;
                opPreLength = 2;
                return true;
            case TokenType.PipeSymbol when tokenQueue.PeekType(1) is TokenType.PipeSymbol:
                op = BinaryExpressionOperator.LogicalOr;
                opPreLength = 2;
                return true;
            case TokenType.QuestionMarkSymbol when tokenQueue.PeekType(1) is TokenType.QuestionMarkSymbol:
                op = BinaryExpressionOperator.Coalesce;
                opPreLength = 2;
                return true;
            case TokenType.OpeningSquareBracket:
                op = BinaryExpressionOperator.Index;
                opPostLength = 1;
                return true;
            default:
                throw new ParserException($"Unexpected operator: {tokenQueue.Peek()}");
        }
    }
}