using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates")]
public class CharacterSource(IEnumerable<char> chars)
{
    private readonly Queue<char> charQueue = new(chars);

    public Uri? DocumentUri { get; init; }

    public int Column { get; private set; } = 1;

    public int Line { get; private set; } = 1;

    public static implicit operator CharacterSource(string source) => new(source);

    public static implicit operator CharacterSource(FileSystemInfo file) => FromFile(file);

    public static CharacterSource FromFile(FileSystemInfo file)
    {
        var text = File.ReadAllText(file.FullName);

        return new CharacterSource(text) { DocumentUri = new Uri(file.FullName) };
    }

    public static async Task<CharacterSource> FromFileAsync(FileSystemInfo file, CancellationToken cancellationToken = default)
    {
        var text = await File.ReadAllTextAsync(file.FullName, cancellationToken);

        return new CharacterSource(text) { DocumentUri = new Uri(file.FullName) };
    }

    public bool TryConsume(out char character)
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

    private bool TryPeek(int offset, out char character)
    {
        character = charQueue.Skip(offset).FirstOrDefault();

        return character != 0;
    }

    public bool TryPeek(out char character) => TryPeek(0, out character);

    public IEnumerable<char> ConsumeUntil(char c)
    {
        var characters = new List<char>();

        while (TryPeek(out var nextChar))
        {
            if (nextChar == c)
                break;

            _ = TryConsume(out var character);

            characters.Add(character);
        }

        return characters;
    }
}
