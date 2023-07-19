using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using HILFE.Parsing.Statements;
using HILFE.Tokenizing;

namespace HILFE.Parsing;

public class Parser
{
    private readonly Queue<Token> tokenQueue = new();

    public void Add(IEnumerable<Token> tokens)
    {
        foreach (var token in tokens)
            tokenQueue.Enqueue(token);
    }

    public async Task AddAsync(IAsyncEnumerable<Token> tokens, CancellationToken cancellationToken = default)
    {
        await foreach (var token in tokens.WithCancellation(cancellationToken))
            tokenQueue.Enqueue(token);
    }

    public async IAsyncEnumerable<Statement> ParseAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (tokenQueue.TryDequeue(out var token))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (token.Type == TokenType.TypeName)
            {
                // variable declaration: <type name> <identifier> = <expression>
                var identifier = Expect(TokenType.Identifier);
                Expect(TokenType.EqualsSymbol);
                var expressionTokens = ReadUntil(TokenType.NewLine);
                Expect(TokenType.NewLine);

                var expression = ParseExpression(expressionTokens);
                yield return new VariableDeclarationStatement(token, identifier, expression);
            }
            else if (token.Type == TokenType.Identifier)
            {
                if (IsNext(TokenType.ExpressionOpener))
                {
                    // function call: <identifier>([<expression>[, <expression>]*])
                    Expect(TokenType.ExpressionOpener);
                    var argsTokens = ReadUntil(TokenType.ExpressionCloser);
                    Expect(TokenType.ExpressionCloser);

                    var args = ParseExpressions(argsTokens);
                    yield return new FunctionCallStatement(token, args);
                }
                else
                {
                    // variable assignment: <identifier> = <expression>
                    Expect(TokenType.EqualsSymbol);
                    var expressionTokens = ReadUntil(TokenType.NewLine);
                    Expect(TokenType.NewLine);

                    var expression = ParseExpression(expressionTokens);
                    yield return new VariableAssignmentStatement(token, expression);
                }
            }
            else if (token.Type == TokenType.CodeBlockOpener)
            {
                // TODO: scope enter: { [statement]* }
                throw new NotImplementedException("Can't handle CodeBlockOpener yet");
            }
            else if (token.Type == TokenType.IfKeyword)
            {
                // TODO: branching: if (<expression>) { [statement]* } [else[if (<expression>)] { [statement]* } ]
                throw new NotImplementedException("Can't handle IfKeyword yet");
            }
            else if (token.Type == TokenType.WhileKeyword)
            {
                // looping: while (<expression>) { [statement]* }
                Expect(TokenType.ExpressionOpener);
                var expressionTokens = ReadUntil(TokenType.ExpressionCloser);
                Expect(TokenType.ExpressionCloser);
                Expect(TokenType.CodeBlockOpener);
                var statements = await ParseAsync(cancellationToken).ToListAsync(cancellationToken);
                // CodeBlockCloser should have been read already

                var condition = ParseExpression(expressionTokens);
                yield return new WhileStatement(condition, statements);
            }
            else if (token.Type == TokenType.CodeBlockOpener)
            {
                // anonymous code block: { [statement]* }
                var statements = await ParseAsync(cancellationToken).ToListAsync(cancellationToken);
                Expect(TokenType.CodeBlockCloser);

                yield return new AnonymousCodeBlockStatement(statements);
            }
            else if (token.Type == TokenType.CodeBlockCloser)
                yield break;
            else if (token.Type is not TokenType.Whitespace and not TokenType.NewLine)
                throw new UnexpectedTokenException(token, TokenType.TypeName, TokenType.Identifier, TokenType.Whitespace, TokenType.NewLine, TokenType.IfKeyword, TokenType.WhileKeyword, TokenType.CodeBlockOpener, TokenType.CodeBlockCloser);
        }
    }

    private Token Expect(params TokenType[] allowed)
    {
        Token? token;
        do
        {
            if (!tokenQueue.TryDequeue(out token))
                throw new UnexpectedEndOfInputException($"Expected token (allowed types: {string.Join(',', allowed)}), but token queue was empty");
        } while (token.Type is TokenType.Whitespace);

        if (!allowed.Contains(token.Type))
            throw new UnexpectedTokenException(token, allowed);

        return token;
    }

    private IReadOnlyList<Token> ReadUntil(TokenType exitType, bool skipWhitespace = true)
    {
        var tokens = new List<Token>();
        var expressionDepth = 0;

        while (TryPeekType(out var nextType))
        {
            if (expressionDepth == 0 && nextType == exitType)
                break;

            var token = tokenQueue.Dequeue();
            if (!skipWhitespace || nextType != TokenType.Whitespace)
                tokens.Add(token);

            if (token.Type is TokenType.ExpressionOpener)
                expressionDepth++;
            else if (token.Type is TokenType.ExpressionCloser)
                expressionDepth--;
        }

        return tokens;
    }

    private bool TryPeekType([NotNullWhen(true)] out TokenType? type)
    {
        type = null;

        if (!tokenQueue.TryPeek(out var token))
            return false;

        type = token.Type;
        return true;
    }

    private bool IsNext(TokenType type) => TryPeekType(out var nextType) && nextType == type;

    public static Expression ParseExpression(IReadOnlyList<Token> tokens)
    {
        var currentPosition = 0;

        Expression ParseBinaryExpression(int parentPrecedence = 0)
        {
            var left = ParsePrimaryExpression();

            while (currentPosition < tokens.Count && BinaryOperatorPrecedence(tokens[currentPosition].Type) >= parentPrecedence)
            {
                var op = tokens[currentPosition++];
                var right = ParseBinaryExpression(BinaryOperatorPrecedence(op.Type) + 1);

                switch (op.Type)
                {
                    case TokenType.PlusSymbol:
                        left = Expression.Add(left, right);
                        break;
                    case TokenType.MinusSymbol:
                        left = Expression.Subtract(left, right);
                        break;
                    case TokenType.AsteriskSymbol:
                        left = Expression.Multiply(left, right);
                        break;
                    case TokenType.SlashSymbol:
                        left = Expression.Divide(left, right);
                        break;
                    case TokenType.PercentSymbol:
                        left = Expression.Modulo(left, right);
                        break;
                    // TODO: Add more binary operators here
                    default:
                        throw new InvalidOperationException("Invalid binary operator.");
                }
            }

            return left;
        }

        Expression ParsePrimaryExpression()
        {
            var currentToken = tokens[currentPosition++];

            if (currentToken.Type == TokenType.Identifier)
            {
                // either 'identifier' or 'function(arg, arg2)'

                if (tokens.Count <= currentPosition || tokens[currentPosition + 1].Type != TokenType.ExpressionOpener)
                    return new Expression.VariableExpression(currentToken);

                Match(TokenType.ExpressionOpener);

                var innerExpressionTokens = tokens.Skip(currentPosition).TakeWhile(t => t.Type != TokenType.ExpressionCloser).ToList();
                var expressions = ParseExpressions(innerExpressionTokens);
                currentPosition += innerExpressionTokens.Count;

                Match(TokenType.ExpressionCloser);

                return new Expression.FunctionCallExpression(currentToken, expressions);

            }

            if (currentToken.Type == TokenType.LiteralNumber)
            {
                if (double.TryParse(currentToken.Value, out var value))
                {
                    return Expression.Constant(value);
                }

                throw new FormatException("Invalid number format.");
            }

            if (currentToken.Type == TokenType.LiteralBoolean)
            {
                if (bool.TryParse(currentToken.Value, out var value))
                {
                    return Expression.Constant(value);
                }

                throw new FormatException("Invalid bool format.");
            }

            if (currentToken.Type == TokenType.LiteralString)
            {
                return Expression.Constant(currentToken.Value);
            }

            if (currentToken.Type == TokenType.ExpressionOpener)
            {
                var expression = ParseBinaryExpression();
                Match(TokenType.ExpressionCloser);
                return expression;
            }

            throw new InvalidOperationException("Invalid expression.");
        }

        int BinaryOperatorPrecedence(TokenType type)
        {
            switch (type)
            {
                case TokenType.PlusSymbol:
                case TokenType.MinusSymbol:
                    return 1;
                case TokenType.AsteriskSymbol:
                case TokenType.SlashSymbol:
                case TokenType.PercentSymbol:
                    return 2;
                // TODO: Add more binary operators and their precedences here
                default:
                    throw new NotImplementedException($"Undefined operator precedence for token type: {type}");
            }
        }

        void Match(TokenType expectedType)
        {
            if (currentPosition >= tokens.Count)
            {
                throw new InvalidOperationException("Unexpected end of expression.");
            }

            var currentToken = tokens[currentPosition++];

            if (currentToken.Type != expectedType)
            {
                throw new InvalidOperationException($"Unexpected token {currentToken.Value}, expected {expectedType}.");
            }
        }

        // Start parsing from the top-level binary expression
        return ParseBinaryExpression();
    }

    private static List<Expression> ParseExpressions(IReadOnlyList<Token> tokens)
    {
        var outputList = new List<Expression>();
        if (tokens.Count == 0)
        {
            return outputList;
        }

        var currentGroup = new List<Token> { tokens[0] };
        Expression currentExpression;

        for (var i = 1; i < tokens.Count; i++)
        {
            if (tokens[i].Type == TokenType.ExpressionSeparator)
            {
                currentExpression = ParseExpression(currentGroup);

                outputList.Add(currentExpression);

                currentGroup = new();
            } else {
                currentGroup.Add(tokens[i]);
            }
        }

        currentExpression = ParseExpression(currentGroup);

        outputList.Add(currentExpression);

        return outputList;
    }
}