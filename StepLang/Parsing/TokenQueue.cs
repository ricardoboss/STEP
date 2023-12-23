using System.Diagnostics.CodeAnalysis;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

/// <summary>
/// A queue data structure specifically for iterating over <see cref="Token"/>s.
/// </summary>
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix")]
public class TokenQueue
{
    private readonly LinkedList<Token> tokenList;

    public TokenQueue() => tokenList = [];

    public TokenQueue(IEnumerable<Token> tokens) => tokenList = new(tokens);

    public bool IgnoreMeaningless { get; set; }

    public Token? LastToken { get; private set; }

    /// <summary>
    /// Tries to remove a <see cref="Token"/> from the beginning of the queue.
    /// </summary>
    /// <param name="token">The <see cref="Token"/> that was removed.</param>
    /// <returns>True if a <see cref="Token"/> was removed, false otherwise.</returns>
    public bool TryDequeue([NotNullWhen(true)] out Token? token)
    {
        token = null;
        if (tokenList.Count == 0)
            return false;

        var skip = 0;
        do
        {
            token = tokenList.Skip(skip).FirstOrDefault();
            skip++;
        } while (IgnoreMeaningless && !(token?.Type.HasMeaning() ?? true));

        if (token is null)
            return false;

        LastToken = token;

        for (; skip > 0; skip--)
            tokenList.RemoveFirst();

        return true;
    }

    /// <summary>
    /// Removes a <see cref="Token"/> from the beginning of the queue or throws an exception if the queue is empty.
    /// </summary>
    /// <returns>The <see cref="Token"/> that was removed.</returns>
    /// <exception cref="UnexpectedEndOfTokensException">Thrown if the queue is empty.</exception>
    public Token Dequeue()
    {
        if (!TryDequeue(out var token))
            throw new UnexpectedEndOfTokensException(LastToken?.Location);

        return token;
    }

    /// <summary>
    /// Removes a collection of <see cref="Token"/>s from the beginning of the queue or throws an exception if the queue is empty before <paramref name="count"/> <see cref="Token"/>s have been removed.
    /// </summary>
    /// <param name="count">The number of <see cref="Token"/>s to remove.</param>
    /// <returns>The collection of <see cref="Token"/>s that were removed in the order they were removed.</returns>
    public Token[] Dequeue(int count)
    {
        var tokens = new Token[count];

        for (var i = 0; i < count; i++)
            tokens[i] = Dequeue();

        return tokens;
    }

    public Token Peek(int offset = 0)
    {
        var source = tokenList.AsEnumerable();
        if (IgnoreMeaningless)
            source = source.Where(t => t.Type.HasMeaning());

        return source.Skip(offset).First();
    }

    /// <summary>
    /// Gets the <see cref="TokenType"/> of a <see cref="Token"/> at the <paramref name="offset"/> from the beginning of the queue without removing it.
    /// </summary>
    /// <param name="offset">The offset from the beginning of the queue to get the <see cref="Token"/> from.</param>
    /// <returns>The <see cref="TokenType"/> of the <see cref="Token"/> at the <paramref name="offset"/> from the beginning of the queue.</returns>
    /// <exception cref="UnexpectedEndOfTokensException">Thrown if the queue is empty before the <paramref name="offset"/> is reached.</exception>
    public TokenType PeekType(int offset = 0)
    {
        var token = Peek(offset);

        return token.Type;
    }

    /// <summary>
    /// Removes a <see cref="Token"/> from the beginning of the queue or throws an exception if the queue is empty.
    /// </summary>
    /// <param name="allowed">The <see cref="TokenType"/>s that are allowed to be removed.</param>
    /// <returns>The <see cref="Token"/> that was removed.</returns>
    /// <exception cref="UnexpectedEndOfTokensException">Thrown if the queue is empty.</exception>
    /// <exception cref="UnexpectedTokenException">Thrown if the <see cref="Token"/> at the beginning of the queue is not one of the <paramref name="allowed"/> <see cref="TokenType"/>s.</exception>
    public Token Dequeue(params TokenType[] allowed)
    {
        Token? token;
        do
        {
            if (TryDequeue(out token))
                continue;

            var typeInfo = allowed.Length switch
            {
                0 => "any token",
                1 => $"a {allowed[0].ToDisplay()}",
                _ => $"any one of {string.Join(',', allowed.Select(TokenTypes.ToDisplay))}",
            };

            throw new UnexpectedEndOfTokensException(LastToken?.Location, $"Expected {typeInfo}");
        } while (!token.Type.HasMeaning() && IgnoreMeaningless);

        if (!allowed.Contains(token.Type))
            throw new UnexpectedTokenException(token, allowed);

        return token;
    }
}