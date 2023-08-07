using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix")]
public class CharacterQueue
{
    private readonly Queue<char> charQueue = new();

    public int Column { get; private set; }

    public int Line { get; private set; }

    public void Enqueue(char c) => charQueue.Enqueue(c);
    
    public void Enqueue(IEnumerable<char> chars)
    {
        foreach (var c in chars)
            charQueue.Enqueue(c);
    }

    public bool TryDequeue(out char character)
    {
        var success = charQueue.TryDequeue(out character);
        if (!success)
            return false;

        if (character == '\n')
        {
            Line++;
            Column = 0;
        }
        else
        {
            Column++;
        }

        return true;
    }

    public char Dequeue()
    {
        if (!TryDequeue(out var character))
            throw new UnexpectedEndOfCharactersException();

        return character;
    }

    public bool TryPeek(int offset, out char character)
    {
        character = charQueue.Skip(offset).FirstOrDefault();

        return character != default;
    }

    public bool TryPeek(out char character) => TryPeek(0, out character);

    public IEnumerable<char> DequeueUntil(char c)
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