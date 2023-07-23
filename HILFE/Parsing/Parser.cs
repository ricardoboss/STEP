using System.Diagnostics;
using System.Runtime.CompilerServices;
using HILFE.Parsing.Expressions;
using HILFE.Parsing.Statements;
using HILFE.Tokenizing;

namespace HILFE.Parsing;

public class Parser
{
    private readonly TokenQueue tokenQueue = new();

    public void Add(IEnumerable<Token> tokens)
    {
        tokenQueue.Enqueue(tokens);
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

                var expression = ExpressionParser.Parse(expressionTokens);
                yield return new VariableDeclarationStatement(token, identifier, expression);
            }
            else if (token.Type == TokenType.Identifier)
            {
                var nextType = tokenQueue.PeekType();
                switch (nextType)
                {
                    case TokenType.ExpressionOpener:
                    {
                        // function call: <identifier>([<expression>[, <expression>]*])
                        Expect(TokenType.ExpressionOpener);
                        var argsTokens = ReadUntil(TokenType.ExpressionCloser);
                        Expect(TokenType.ExpressionCloser);

                        var args = ExpressionParser.ParseExpressions(argsTokens).ToList();
                        yield return new FunctionCallStatement(token, args);

                        break;
                    }
                    case { } when nextType.IsMathematicalOperation():
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
                        var expression = ExpressionParser.Parse(new[] {token, operationToken}.Concat(valueExpTokens).ToArray());

                        yield return new VariableAssignmentStatement(token, expression);

                        break;
                    }
                    default:
                    {
                        // variable assignment: <identifier> = <expression>
                        Expect(TokenType.EqualsSymbol);
                        var expressionTokens = ReadUntil(TokenType.NewLine);
                        Expect(TokenType.NewLine);

                        var expression = ExpressionParser.Parse(expressionTokens);
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

                var condition = ExpressionParser.Parse(expressionTokens);
                var statements = await ParseStatements(trueBranchTokens, cancellationToken).ToListAsync(cancellationToken);

                if (tokenQueue.PeekType() is not TokenType.ElseKeyword)
                {
                    yield return new IfStatement(condition, statements);
                }
                else
                {
                    Expect(TokenType.ElseKeyword);

                    IReadOnlyList<Token>? elseExpressionTokens = null;
                    if (tokenQueue.PeekType() is TokenType.IfKeyword)
                    {
                        Expect(TokenType.ExpressionOpener);
                        elseExpressionTokens = ReadUntil(TokenType.ExpressionCloser);
                        Expect(TokenType.ExpressionCloser);
                    }

                    Expect(TokenType.CodeBlockOpener);
                    var falseBranchTokens = ReadUntil(TokenType.CodeBlockCloser);
                    Expect(TokenType.CodeBlockCloser);

                    var elseExpression =
                        elseExpressionTokens is not null ? ExpressionParser.Parse(elseExpressionTokens) : null;

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

                var condition = ExpressionParser.Parse(expressionTokens);
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

        if (tokenQueue.IsNotEmpty)
        {
            throw new ParserException("Queue was not consumed till the end");
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

        while (tokenQueue.TryPeekType(out var nextType))
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

    private static IAsyncEnumerable<Statement> ParseStatements(IEnumerable<Token> tokens, CancellationToken cancellationToken)
    {
        // TODO: ugly? need to be able to parse tokens after they have been taken out of the queue

        var innerParser = new Parser();

        innerParser.Add(tokens);

        return innerParser.ParseAsync(cancellationToken);
    }
}