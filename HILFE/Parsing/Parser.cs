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
                // TODO: add function call evaluation as value
                var identifier = Expect(TokenType.Identifier);
                Expect(TokenType.EqualsSymbol);
                var value = ReadUntil(TokenType.NewLine);
                Expect(TokenType.NewLine);

                yield return new VariableDeclarationStatement(token, identifier, value);
            }
            else if (token.Type == TokenType.Identifier)
            {
                if (IsNext(TokenType.ExpressionOpener))
                {
                    // function call: <identifier>([<expression>[, <expression>]*])
                    Expect(TokenType.ExpressionOpener);
                    var args = ReadUntil(TokenType.ExpressionCloser);
                    Expect(TokenType.ExpressionCloser);

                    yield return new FunctionCallStatement(token, args);
                }
                else
                {
                    // variable assignment: <identifier> = <expression>
                    // TODO: add function call evaluation as value
                    Expect(TokenType.EqualsSymbol);
                    var value = ReadUntil(TokenType.NewLine);

                    yield return new VariableAssignmentStatement(token, value);
                }
            }
            else if (token.Type == TokenType.CodeBlockOpener)
            {
                // TODO: scope enter: { [statement]* }
            }
            else if (token.Type == TokenType.IfKeyword)
            {
                // TODO: branching: if (<expression>) { [statement]* } [else[if (<expression>)] { [statement]* } ]
            }
            else if (token.Type == TokenType.WhileKeyword)
            {
                // TODO: looping while (<expression>) { [statement]* }
            }
            else if (token.Type is TokenType.Whitespace or TokenType.NewLine)
                yield return new EmptyStatement();
            else
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
        } while (token.Type == TokenType.Whitespace);

        if (!allowed.Contains(token.Type))
            throw new UnexpectedTokenException(token, allowed);

        return token;
    }

    private IReadOnlyList<Token> ReadUntil(TokenType exitType, bool skipWhitespace = true)
    {
        var tokens = new List<Token>();

        while (TryPeekType(out var nextType) && nextType != exitType)
        {
            var token = tokenQueue.Dequeue();
            if (!skipWhitespace || nextType != TokenType.Whitespace)
                tokens.Add(token);
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
}