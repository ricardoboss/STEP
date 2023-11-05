using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix")]
public class CharacterQueue
{
    private readonly Queue<char> charQueue = new();

    public int Column { get; private set; }

    public int Line { get; private set; }

    private FileSystemInfo? file;
    public FileSystemInfo? File
    {
        get => file;
        set
        {
            file = value;
            Line = 1;
            Column = 1;
        }
    }

    public TokenLocation CurrentLocation => new(File, Line, Column);

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
            Column = 1;
        }
        else
        {
            Column++;
        }

        return true;
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

        while (TryPeek(out var nextChar))
        {
            if (nextChar == c || !TryDequeue(out var character))
                break;

            characters.Add(character);
        }

        return characters;
    }
}