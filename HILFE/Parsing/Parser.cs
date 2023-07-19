using System.Diagnostics;
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
                var nextType = PeekType();
                if (nextType is TokenType.ExpressionOpener)
                {
                    // function call: <identifier>([<expression>[, <expression>]*])
                    Expect(TokenType.ExpressionOpener);
                    var argsTokens = ReadUntil(TokenType.ExpressionCloser);
                    Expect(TokenType.ExpressionCloser);

                    var args = ParseExpressions(argsTokens);
                    yield return new FunctionCallStatement(token, args);
                }
                else if (nextType.IsMathematicalOperation())
                {
                    // shortcut setter: <identifier>++ / <identifier>-- / <identifier> *= <expression> / <identifier> /= <expression>
                    var variableExp = new Expression.VariableExpression(token);
                    var operationToken = GetNext();
                    Expression valueExp;

                    var valueDenominator = Expect(TokenType.PlusSymbol,
                        TokenType.MinusSymbol, TokenType.AsteriskSymbol, TokenType.SlashSymbol,
                        TokenType.PercentSymbol, TokenType.EqualsSymbol);

                    if (valueDenominator.Type.IsMathematicalOperation())
                    {
                        // mathematical operation must be the same type as the initial operation (i.e. +- or *- is forbidden)
                        if (operationToken.Type != valueDenominator.Type)
                            throw new UnexpectedTokenException(valueDenominator, operationToken.Type);

                        valueExp = new Expression.ConstantExpression("double", 1);
                    }
                    else
                    {
                        Debug.Assert(valueDenominator.Type is TokenType.EqualsSymbol);

                        valueExp = ParseExpression(ReadUntil(TokenType.NewLine));
                        Expect(TokenType.NewLine);
                    }

                    var expression = Expression.FromSymbol(operationToken, variableExp, valueExp);

                    yield return new VariableAssignmentStatement(token, expression);
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
                // CodeBlockCloser should have been read already

                yield return new AnonymousCodeBlockStatement(statements);
            }
            else if (token.Type == TokenType.CodeBlockCloser)
                yield break;
            else if (token.Type is not TokenType.Whitespace and not TokenType.NewLine)
                throw new UnexpectedTokenException(token, TokenType.TypeName, TokenType.Identifier, TokenType.Whitespace, TokenType.NewLine, TokenType.IfKeyword, TokenType.WhileKeyword, TokenType.CodeBlockOpener, TokenType.CodeBlockCloser);
        }
    }

    private Token GetNext(params TokenType[] allowed)
    {
        Token? token;
        do
        {
            if (tokenQueue.TryDequeue(out token))
                continue;

            var typeInfo = allowed.Length == 0 ? "any token" : $"a token (allowed types: {string.Join(',', allowed)})";

            throw new UnexpectedEndOfInputException($"Expected {typeInfo}, but token queue was empty");
        } while (token.Type is TokenType.Whitespace);

        return token;
    }

    private Token Expect(params TokenType[] allowed)
    {
        var token = GetNext(allowed);
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

            switch (token.Type)
            {
                case TokenType.ExpressionOpener:
                    expressionDepth++;
                    break;
                case TokenType.ExpressionCloser:
                    expressionDepth--;
                    break;
            }
        }

        return tokens;
    }

    private TokenType PeekType()
    {
        return tokenQueue.Peek().Type;
    }

    private bool TryPeekType([NotNullWhen(true)] out TokenType? type)
    {
        type = null;

        if (!tokenQueue.TryPeek(out var token))
            return false;

        type = token.Type;
        return true;
    }

    public static Expression ParseExpression(IReadOnlyList<Token> tokens)
    {
        var currentPosition = 0;

        // Start parsing from the top-level binary expression
        return ParseBinaryExpression();

        Expression ParseBinaryExpression(int parentPrecedence = 0)
        {
            var left = ParsePrimaryExpression();

            while (currentPosition < tokens.Count && BinaryOperatorPrecedence(tokens[currentPosition].Type) >= parentPrecedence)
            {
                var op = tokens[currentPosition++];

                // FIXME: this currently prevents >= and <= operators from being implemented
                if (tokens[currentPosition].Type is TokenType.EqualsSymbol)
                    currentPosition++;

                var right = ParseBinaryExpression(BinaryOperatorPrecedence(op.Type) + 1);

                left = Expression.FromSymbol(op, left, right);
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

                throw new FormatException($"Invalid bool format: {currentToken.Value}");
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

            throw new InvalidOperationException($"Invalid expression. Got token: {currentToken}");
        }

        int BinaryOperatorPrecedence(TokenType type)
        {
            switch (type)
            {
                case TokenType.LessThanSymbol:
                case TokenType.GreaterThanSymbol:
                case TokenType.EqualsSymbol:
                    return 1;
                case TokenType.PlusSymbol:
                case TokenType.MinusSymbol:
                    return 2;
                case TokenType.AsteriskSymbol:
                case TokenType.SlashSymbol:
                case TokenType.PercentSymbol:
                    return 3;
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