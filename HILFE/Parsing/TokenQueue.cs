using System.Collections;
using System.Diagnostics.CodeAnalysis;
using HILFE.Tokenizing;

namespace HILFE.Parsing;

[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix")]
public class TokenQueue
{
    private readonly LinkedList<Token> tokenList = new();

    public void Enqueue(Token token) => tokenList.AddLast(token);

    public void Enqueue(IEnumerable<Token> tokens)
    {
        foreach (var token in tokens)
            tokenList.AddLast(token);
    }

    public void Prepend(Token token) => tokenList.AddFirst(token);

    public bool TryDequeue([NotNullWhen(true)] out Token? token)
    {
        token = tokenList.First?.Value;
        if (token is null)
            return false;

        tokenList.RemoveFirst();

        return true;
    }

    public Token Dequeue()
    {
        if (!TryDequeue(out var token))
            throw new UnexpectedEndOfInputException("Expected a token, but token queue was empty");

        return token;
    }

    public Token[] Dequeue(int count)
    {
        var tokens = new Token[count];

        for (var i = 0; i < count; i++)
            tokens[i] = Dequeue();

        return tokens;
    }

    public bool TryPeek(int offset, [NotNullWhen(true)] out Token? token)
    {
         token = tokenList.Skip(offset).FirstOrDefault();

         return token is not null;
    }

    public bool TryPeekType([NotNullWhen(true)] out TokenType? type) => TryPeekType(0, out type);

    public bool TryPeekType(int offset, [NotNullWhen(true)] out TokenType? type)
    {
        type = null;
        if (!TryPeek(offset, out var token))
            return false;

        type = token.Type;

        return true;
    }

    public Token Peek(int offset = 0)
    {
        if (!TryPeek(offset, out var token))
            throw new ParserException("Unexpected end of token queue");

        return token;
    }

    public TokenType PeekType(int offset = 0)
    {
        TokenType? type;
        do
        {
            if (!TryPeekType(offset, out type))
                throw new ParserException("Unexpected end of token queue");

            offset++;
        } while (type is TokenType.Whitespace or TokenType.LineComment);

        return type.Value;
    }

    public Token Dequeue(params TokenType[] allowed)
    {
        Token? token;
        do
        {
            if (TryDequeue(out token))
                continue;

            var typeInfo = allowed.Length == 0 ? "any token" : $"a token (allowed types: {string.Join(',', allowed)})";

            throw new UnexpectedEndOfInputException($"Expected {typeInfo}, but token queue was empty");
        } while (token.Type is TokenType.Whitespace or TokenType.LineComment);

        if (!allowed.Contains(token.Type))
            throw new UnexpectedTokenException(token, allowed);

        return token;
    }

    public IReadOnlyList<Token> DequeueUntil(TokenType exitType)
    {
        var tokens = new List<Token>();
        var expressionDepth = 0;
        var codeBlockDepth = 0;

        while (TryPeekType(out var nextType))
        {
            if (expressionDepth == 0 && codeBlockDepth == 0 && nextType == exitType)
                break;

            var token = Dequeue();
            if (nextType is not TokenType.Whitespace and not TokenType.LineComment)
                tokens.Add(token);

            switch (nextType)
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
            }
        }

        return tokens;
    }

    public bool IsEmpty => tokenList.Count == 0;

    public bool IsNotEmpty => tokenList.Count > 0;

    public IEnumerable<Token> Consume() => new ConsumingTokenQueue(this);

    private sealed class ConsumingTokenQueue : IEnumerable<Token>
    {
        private readonly TokenQueue tokenQueue;

        public ConsumingTokenQueue(TokenQueue tokenQueue) => this.tokenQueue = tokenQueue;

        /// <inheritdoc />
        public IEnumerator<Token> GetEnumerator() => new TokenQueueConsumer(tokenQueue);

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => new TokenQueueConsumer(tokenQueue);
    }

    private sealed class TokenQueueConsumer : IEnumerator<Token>
    {
        private readonly TokenQueue tokenQueue;

        public TokenQueueConsumer(TokenQueue tokenQueue) => this.tokenQueue = tokenQueue;

        /// <inheritdoc />
        public bool MoveNext() => tokenQueue.TryDequeue(out _);

        /// <inheritdoc />
        public Token Current => tokenQueue.Peek();

        /// <inheritdoc />
        object IEnumerator.Current => Current;

        /// <inheritdoc />
        public void Reset()
        {
            // do nothing
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // do nothing
        }
    }
}