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
                switch (nextType)
                {
                    case TokenType.ExpressionOpener:
                    {
                        // function call: <identifier>([<expression>[, <expression>]*])
                        Expect(TokenType.ExpressionOpener);
                        var argsTokens = ReadUntil(TokenType.ExpressionCloser);
                        Expect(TokenType.ExpressionCloser);

                        var args = ParseExpressions(argsTokens);
                        yield return new FunctionCallStatement(token, args);

                        break;
                    }
                    case { } maybeMathematicalOperator when maybeMathematicalOperator.IsMathematicalOperation():
                    {
                        // shortcut setter: <identifier>++ / <identifier>-- / <identifier> *= <expression> / <identifier> /= <expression>
                        var operationToken = GetNext();

                        var valueDenominator = Expect(TokenType.PlusSymbol,
                            TokenType.MinusSymbol, TokenType.AsteriskSymbol, TokenType.SlashSymbol,
                            TokenType.PercentSymbol, TokenType.EqualsSymbol);

                        IReadOnlyList<Token> valueExpTokens;
                        if (valueDenominator.Type.IsMathematicalOperation())
                        {
                            // mathematical operation must be the same type as the initial operation (i.e. +- or *- is forbidden)
                            if (operationToken.Type != valueDenominator.Type)
                                throw new UnexpectedTokenException(valueDenominator, operationToken.Type);

                            valueExpTokens = new Token[] { new(TokenType.LiteralNumber, "1") };
                        }
                        else
                        {
                            Debug.Assert(valueDenominator.Type is TokenType.EqualsSymbol);

                            valueExpTokens = ReadUntil(TokenType.NewLine);
                            Expect(TokenType.NewLine);
                        }

                        // TODO: ugly. Move expression parsing to another class
                        var expression = ParseExpression(new[] {token, operationToken}.Concat(valueExpTokens).ToArray());

                        yield return new VariableAssignmentStatement(token, expression);

                        break;
                    }
                    default:
                    {
                        // variable assignment: <identifier> = <expression>
                        Expect(TokenType.EqualsSymbol);
                        var expressionTokens = ReadUntil(TokenType.NewLine);
                        Expect(TokenType.NewLine);

                        var expression = ParseExpression(expressionTokens);
                        yield return new VariableAssignmentStatement(token, expression);

                        break;
                    }
                }
            }
            else if (token.Type == TokenType.IfKeyword)
            {
                // branching: if (<expression>) { [statement]* } [else[if (<expression>)] { [statement]* } ]

                Expect(TokenType.ExpressionOpener);
                var expressionTokens = ReadUntil(TokenType.ExpressionCloser);
                Expect(TokenType.ExpressionCloser);

                Expect(TokenType.CodeBlockOpener);
                var trueBranchTokens = ReadUntil(TokenType.CodeBlockCloser);
                Expect(TokenType.CodeBlockCloser);

                var condition = ParseExpression(expressionTokens);
                var statements = await ParseStatements(trueBranchTokens, cancellationToken).ToListAsync(cancellationToken);

                if (PeekType() is not TokenType.ElseKeyword)
                {
                    yield return new IfStatement(condition, statements);
                }
                else
                {
                    Expect(TokenType.ElseKeyword);

                    IReadOnlyList<Token>? elseExpressionTokens = null;
                    if (PeekType() is TokenType.IfKeyword)
                    {
                        Expect(TokenType.ExpressionOpener);
                        elseExpressionTokens = ReadUntil(TokenType.ExpressionCloser);
                        Expect(TokenType.ExpressionCloser);
                    }

                    Expect(TokenType.CodeBlockOpener);
                    var falseBranchTokens = ReadUntil(TokenType.CodeBlockCloser);
                    Expect(TokenType.CodeBlockCloser);

                    var elseExpression =
                        elseExpressionTokens is not null ? ParseExpression(elseExpressionTokens) : null;

                    var elseStatements = await ParseStatements(falseBranchTokens, cancellationToken)
                        .ToListAsync(cancellationToken);

                    yield return new IfElseStatement(condition, statements, elseExpression, elseStatements);
                }
            }
            else if (token.Type == TokenType.WhileKeyword)
            {
                // looping: while (<expression>) { [statement]* }
                Expect(TokenType.ExpressionOpener);
                var expressionTokens = ReadUntil(TokenType.ExpressionCloser);
                Expect(TokenType.ExpressionCloser);

                Expect(TokenType.CodeBlockOpener);
                var statementsTokens = ReadUntil(TokenType.CodeBlockCloser);
                Expect(TokenType.CodeBlockCloser);

                var condition = ParseExpression(expressionTokens);
                var statements = await ParseStatements(statementsTokens, cancellationToken).ToListAsync(cancellationToken);

                yield return new WhileStatement(condition, statements);
            }
            else if (token.Type == TokenType.CodeBlockOpener)
            {
                // anonymous code block: { [statement]* }
                var statementsTokens = ReadUntil(TokenType.CodeBlockCloser);
                Expect(TokenType.CodeBlockCloser);

                var statements = await ParseStatements(statementsTokens, cancellationToken).ToListAsync(cancellationToken);

                yield return new AnonymousCodeBlockStatement(statements);
            }
            else if (token.Type == TokenType.CodeBlockCloser)
                yield break;
            else if (token.Type is not TokenType.Whitespace and not TokenType.NewLine)
                throw new UnexpectedTokenException(token, TokenType.TypeName, TokenType.Identifier, TokenType.Whitespace, TokenType.NewLine, TokenType.IfKeyword, TokenType.WhileKeyword, TokenType.CodeBlockOpener, TokenType.CodeBlockCloser);
        }

        if (tokenQueue.Count > 0)
        {
            throw new ParserException($"Queue was not consumed till the end ({tokenQueue.Count} tokens left)");
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
        var codeBlockDepth = 0;

        while (TryPeekType(out var nextType))
        {
            if (expressionDepth == 0 && codeBlockDepth == 0 && nextType == exitType)
                break;

            var token = tokenQueue.Dequeue();
            if (!skipWhitespace || nextType != TokenType.Whitespace)
                tokens.Add(token);

            switch (nextType)
            {
                case TokenType.ExpressionOpener:
                    expressionDepth++;
                    break;
                case TokenType.ExpressionCloser:
                    expressionDepth--;
                    break;
                case TokenType.CodeBlockOpener:
                    codeBlockDepth++;
                    break;
                case TokenType.CodeBlockCloser:
                    codeBlockDepth--;
                    break;
            }
        }

        return tokens;
    }

    private TokenType? PeekType(bool skipWhitespace = true)
    {
        return tokenQueue.SkipWhile(t => skipWhitespace && t.Type is TokenType.Whitespace).FirstOrDefault(defaultValue: null)?.Type;
    }

    private bool TryPeekType([NotNullWhen(true)] out TokenType? type)
    {
        type = null;

        if (!tokenQueue.TryPeek(out var token))
            return false;

        type = token.Type;
        return true;
    }

    private static IAsyncEnumerable<Statement> ParseStatements(IEnumerable<Token> tokens, CancellationToken cancellationToken)
    {
        // TODO: ugly? need to be able to parse tokens after they have been taken out of the queue

        var innerParser = new Parser();

        innerParser.Add(tokens);

        return innerParser.ParseAsync(cancellationToken);
    }

    private static Expression ParseExpression(IReadOnlyList<Token> tokens)
    {
        var currentPosition = 0;

        // Start parsing from the top-level binary expression
        return ParseBinaryExpression();

        Expression ParseBinaryExpression(int parentPrecedence = 0)
        {
            var left = ParsePrimaryExpression();
            BinaryExpressionOperator op;

            while (currentPosition < tokens.Count && (op = ParseOperator()).Precedence() >= parentPrecedence)
            {
                var right = ParseBinaryExpression(op.Precedence() + 1);

                left = Expression.FromOperator(op, left, right);
            }

            return left;
        }

        Expression ParsePrimaryExpression()
        {
            var currentToken = tokens[currentPosition++];

            if (currentToken.Type == TokenType.Identifier)
            {
                // either 'identifier' or 'function(arg, arg2)'

                if (tokens.Count <= currentPosition || tokens[currentPosition].Type != TokenType.ExpressionOpener)
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
                // TODO: kinda ugly?
                var innerTokens = tokens.Skip(currentPosition).TakeWhile(t => t.Type != TokenType.ExpressionCloser).ToList();
                var expression = ParseExpression(innerTokens);
                currentPosition += innerTokens.Count;
                Match(TokenType.ExpressionCloser);
                return expression;
            }

            throw new InvalidOperationException($"Invalid expression. Got token: {currentToken}");
        }

        BinaryExpressionOperator ParseOperator()
        {
            var currentToken = tokens[currentPosition++];

            if (currentToken.Type == TokenType.PlusSymbol)
            {
                return BinaryExpressionOperator.Plus;
            }
            
            if (currentToken.Type == TokenType.MinusSymbol)
            {
                return BinaryExpressionOperator.Minus;
            }
            
            if (currentToken.Type == TokenType.AsteriskSymbol)
            {
                return BinaryExpressionOperator.Multiply;
            }
            
            if (currentToken.Type == TokenType.SlashSymbol)
            {
                return BinaryExpressionOperator.Divide;
            }
            
            if (currentToken.Type == TokenType.PercentSymbol)
            {
                return BinaryExpressionOperator.Modulo;
            }

            if (currentToken.Type == TokenType.EqualsSymbol)
            {
                Match(TokenType.EqualsSymbol);

                return BinaryExpressionOperator.Equal;
            }

            if (currentToken.Type == TokenType.ExclamationMarkSymbol)
            {
                Match(TokenType.EqualsSymbol);

                return BinaryExpressionOperator.NotEqual;
            }

            if (currentToken.Type == TokenType.GreaterThanSymbol)
            {
                if (Peek() is TokenType.EqualsSymbol)
                {
                    return BinaryExpressionOperator.GreaterThanOrEqual;
                }

                return BinaryExpressionOperator.GreaterThan;
            }
            
            if (currentToken.Type == TokenType.LessThanSymbol)
            {
                if (Peek() is TokenType.EqualsSymbol)
                {
                    return BinaryExpressionOperator.LessThanOrEqual;
                }
                
                return BinaryExpressionOperator.LessThan;
            }

            if (currentToken.Type == TokenType.AmpersandSymbol)
            {
                Match(TokenType.AmpersandSymbol);

                return BinaryExpressionOperator.LogicalAnd;
            }
            
            if (currentToken.Type == TokenType.PipeSymbol)
            {
                Match(TokenType.PipeSymbol);

                return BinaryExpressionOperator.LogicalOr;
            }

            if (currentToken.Type == TokenType.QuestionMarkSymbol)
            {
                Match(TokenType.QuestionMarkSymbol);

                return BinaryExpressionOperator.Coalesce;
            }

            throw new ParserException("Invalid binary operator: " + currentToken);
        }

        TokenType? Peek()
        {
            if (currentPosition >= tokens.Count)
            {
                return null;
            }

            var nextToken = tokens[currentPosition + 1];
            return nextToken.Type;
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