using System.Globalization;
using System.Runtime.CompilerServices;
using StepLang.Expressions.Results;
using StepLang.Parsing;
using StepLang.Statements;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

public class ExpressionParser
{
    private readonly TokenQueue tokenQueue = new();

    public static async Task<Expression> ParseAsync(IEnumerable<Token> tokens, CancellationToken cancellationToken = default)
    {
        var parser = new ExpressionParser();
        parser.tokenQueue.Enqueue(tokens);
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
                parser.tokenQueue.Enqueue(currentGroup);

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

        parser.tokenQueue.Enqueue(currentGroup);

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
                yield return await FinalizeGroup();
            else
            {
                currentGroup.Enqueue(token);

                UpdateDepths(token.Type);
            }
        }

        yield return await FinalizeGroup();

        yield break;

        void UpdateDepths(TokenType? type)
        {
            switch (type)
            {
                case TokenType.OpeningParentheses:
                    expressionDepth++;
                    return;
                case TokenType.ClosingParentheses:
                    expressionDepth--;
                    return;
                case TokenType.OpeningCurlyBracket:
                    codeBlockDepth++;
                    return;
                case TokenType.ClosingCurlyBracket:
                    codeBlockDepth--;
                    return;
                case TokenType.OpeningSquareBracket:
                    listDepth++;
                    return;
                case TokenType.ClosingSquareBracket:
                    listDepth--;
                    return;
            }
        }

        async Task<KeyValuePair<string, Expression>> FinalizeGroup()
        {
            var keyTokens = currentGroup.DequeueUntil(TokenType.ColonSymbol);
            _ = currentGroup.Dequeue(TokenType.ColonSymbol);
            var valueTokens = currentGroup.DequeueUntil(TokenType.CommaSymbol);

            var keyExpression = await ParseAsync(keyTokens, cancellationToken);

            if (keyExpression is not LiteralExpression { Result: StringResult { Value: { } key } })
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

            var op = PeekOperator(out var opPreLength, out var opPostLength);

            var precedence = op.Precedence();
            if (precedence < parentPrecedence)
                break;

            // dequeue operator tokens before right value
            _ = tokenQueue.Dequeue(opPreLength);

            var right = await ParseBinaryExpression(precedence + 1, cancellationToken);

            // dequeue operator tokens after right value
            _ = tokenQueue.Dequeue(opPostLength);

            left = BinaryExpression.FromOperator(op, left, right);
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
                return LiteralExpression.Number(value);
            case TokenType.LiteralNumber:
                throw new FormatException("Invalid number format.");
            case TokenType.LiteralBoolean when bool.TryParse(currentToken.Value, out var value):
                return LiteralExpression.Bool(value);
            case TokenType.LiteralBoolean:
                throw new FormatException($"Invalid bool format: {currentToken.Value}");
            case TokenType.LiteralString:
                return LiteralExpression.Str(currentToken.StringValue);
            case TokenType.OpeningParentheses:
                var nextType = tokenQueue.PeekType();
                if (nextType is TokenType.ClosingParentheses)
                {
                    if (!tokenQueue.TryPeekType(out var nextNextType) || nextNextType is not TokenType.OpeningCurlyBracket)
                        throw new InvalidExpressionException(currentToken, "An empty pair of parentheses is not a valid expression.");

                    // function declaration

                    tokenQueue.Prepend(currentToken);

                    return await ParseFunctionDefinitionExpression(currentToken, cancellationToken);
                }

                if (nextType is TokenType.TypeName)
                {
                    // function declaration

                    tokenQueue.Prepend(currentToken);

                    return await ParseFunctionDefinitionExpression(currentToken, cancellationToken);
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
            case TokenType.ExclamationMarkSymbol:
                var invertedExpression = await ParseUnaryExpression(cancellationToken);

                return UnaryExpression.Not(invertedExpression);
            case TokenType.MinusSymbol:
                var negatedExpression = await ParseUnaryExpression(cancellationToken);

                return UnaryExpression.Negate(negatedExpression);
            default:
                throw new UnexpectedTokenException(currentToken, TokenType.Identifier, TokenType.LiteralNumber, TokenType.LiteralBoolean, TokenType.LiteralString, TokenType.OpeningParentheses, TokenType.OpeningSquareBracket, TokenType.OpeningCurlyBracket);
        }
    }

    private async Task<Expression> ParseFunctionDefinitionExpression(Token openingParenthesisToken, CancellationToken cancellationToken = default)
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

        var definitionExpression = new FunctionDefinitionExpression(parameters, body)
        {
            Location = openingParenthesisToken.Location,
        };

        if (!tokenQueue.TryPeekType(out var nextNextType) || nextNextType is not TokenType.OpeningParentheses)
            return definitionExpression;

        tokenQueue.Dequeue(TokenType.OpeningParentheses);

        var innerExpressionTokens = tokenQueue.DequeueUntil(TokenType.ClosingParentheses);

        tokenQueue.Dequeue(TokenType.ClosingParentheses);

        var args = await ParseExpressionsAsync(innerExpressionTokens, cancellationToken).ToListAsync(cancellationToken);

        return new DirectFunctionDefinitionCallExpression(definitionExpression, args);
    }

    private BinaryExpressionOperator PeekOperator(out int opPreLength, out int opPostLength)
    {
        opPreLength = 1;
        opPostLength = 0;

        switch (tokenQueue.PeekType())
        {
            case TokenType.PlusSymbol:
                return BinaryExpressionOperator.Plus;
            case TokenType.MinusSymbol:
                return BinaryExpressionOperator.Minus;
            case TokenType.AsteriskSymbol:
                return BinaryExpressionOperator.Multiply;
            case TokenType.SlashSymbol:
                return BinaryExpressionOperator.Divide;
            case TokenType.PercentSymbol:
                return BinaryExpressionOperator.Modulo;
            case TokenType.HatSymbol:
                return BinaryExpressionOperator.Power;
            case TokenType.EqualsSymbol when tokenQueue.PeekType(1) is TokenType.EqualsSymbol:
                opPreLength = 2;
                return BinaryExpressionOperator.Equal;
            case TokenType.ExclamationMarkSymbol when tokenQueue.PeekType(1) is TokenType.EqualsSymbol:
                opPreLength = 2;
                return BinaryExpressionOperator.NotEqual;
            case TokenType.GreaterThanSymbol when tokenQueue.PeekType(1) is TokenType.EqualsSymbol:
                opPreLength = 2;
                return BinaryExpressionOperator.GreaterThanOrEqual;
            case TokenType.GreaterThanSymbol:
                return BinaryExpressionOperator.GreaterThan;
            case TokenType.LessThanSymbol when tokenQueue.PeekType(1) is TokenType.EqualsSymbol:
                opPreLength = 2;
                return BinaryExpressionOperator.LessThanOrEqual;
            case TokenType.LessThanSymbol:
                return BinaryExpressionOperator.LessThan;
            case TokenType.AmpersandSymbol when tokenQueue.PeekType(1) is TokenType.AmpersandSymbol:
                opPreLength = 2;
                return BinaryExpressionOperator.LogicalAnd;
            case TokenType.PipeSymbol when tokenQueue.PeekType(1) is TokenType.PipeSymbol:
                opPreLength = 2;
                return BinaryExpressionOperator.LogicalOr;
            case TokenType.QuestionMarkSymbol when tokenQueue.PeekType(1) is TokenType.QuestionMarkSymbol:
                opPreLength = 2;
                return BinaryExpressionOperator.Coalesce;
            case TokenType.OpeningSquareBracket:
                opPostLength = 1;
                return BinaryExpressionOperator.Index;
            default:
                var nextToken = tokenQueue.Peek();
                throw new UnexpectedTokenException(nextToken, $"The operator '{nextToken.Value}' was not recognized or is not supported");
        }
    }
}