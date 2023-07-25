using System.Diagnostics.CodeAnalysis;

namespace HILFE.Tokenizing;

[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix")]
public class CharacterQueue
{
    private readonly Queue<char> charQueue = new();
    
    public void Enqueue(char c) => charQueue.Enqueue(c);
    
    public void Enqueue(IEnumerable<char> chars)
    {
        foreach (var c in chars)
            charQueue.Enqueue(c);
    }

    public bool TryDequeue(out char character)
    {
        return charQueue.TryDequeue(out character);
    }

    public char Dequeue()
    {
        if (!TryDequeue(out var character))
            throw new TokenizerException($"Unexpected end of {nameof(CharacterQueue)}");

        return character;
    }

    public char[] Dequeue(int count)
    {
        var characters = new char[count];

        for (var i = 0; i < count; i++)
            characters[i] = Dequeue();

        return characters;
    }

    public char Dequeue(params char[] allowed)
    {
        if (!TryDequeue(out var character))
            throw new TokenizerException($"Unexpected end of {nameof(CharacterQueue)}");

        if (!allowed.Contains(character))
            throw new TokenizerException($"Unexpected character '{character}'");

        return character;
    }

    public bool TryPeek(int offset, out char character)
    {
        character = charQueue.Skip(offset).FirstOrDefault();

        return character != default;
    }

    public bool TryPeek(out char character) => TryPeek(0, out character);

    public IReadOnlyList<char> DequeueUntil(char c)
    {
        var characters = new List<char>();

        while (TryPeek(out var character))
        {
            if (character == c)
                break;

            characters.Add(Dequeue());
        }

        return characters;
    }
}