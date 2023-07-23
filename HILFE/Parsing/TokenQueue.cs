using System.Collections;
using System.Diagnostics.CodeAnalysis;
using HILFE.Tokenizing;

namespace HILFE.Parsing;

public class TokenQueue
{
    private readonly LinkedList<Token> tokenList = new();

    public void Enqueue(Token token) => tokenList.AddLast(token);

    public void Enqueue(IEnumerable<Token> tokens)
    {
        foreach (var token in tokens)
            tokenList.AddLast(token);
    }

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
            throw new($"Unexpected end of {nameof(TokenQueue)}");

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
        if (!TryPeekType(offset, out var type))
            throw new ParserException("Unexpected end of token queue");

        return type.Value;
    }

    public Token Ensure(TokenType type, int offset = 0)
    {
        var token = Peek(offset);
        if (token.Type != type)
            throw new ParserException($"Unexpected token {token}, expected {type}");

        return token;
    }

    public Token Expect(TokenType type)
    {
        if (!TryDequeue(out var token))
            throw new ParserException("Unexpected end of token queue");

        if (token.Type != type)
            throw new ParserException($"Unexpected token {token}, expected {type}");

        return token;
    }

    public bool IsEmpty => tokenList.Count == 0;

    public bool IsNotEmpty => tokenList.Count > 0;

    public IEnumerable<Token> Consume() => new ConsumingTokenQueue(this);

    private class ConsumingTokenQueue : IEnumerable<Token>
    {
        private readonly TokenQueue tokenQueue;

        public ConsumingTokenQueue(TokenQueue tokenQueue) => this.tokenQueue = tokenQueue;

        /// <inheritdoc />
        public IEnumerator<Token> GetEnumerator() => new TokenQueueConsumer(tokenQueue);

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => new TokenQueueConsumer(tokenQueue);
    }

    private class TokenQueueConsumer : IEnumerator<Token>
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