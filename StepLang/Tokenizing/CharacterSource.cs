using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates")]
public class CharacterSource(IEnumerable<char> chars)
{
	private readonly Queue<char> charQueue = new(chars);

	public FileSystemInfo? File { get; init; }

	public int Column { get; private set; } = 1;

	public int Line { get; private set; } = 1;

	public static implicit operator CharacterSource(string source)
	{
		return new CharacterSource(source);
	}

	public static implicit operator CharacterSource(FileSystemInfo file)
	{
		return FromFile(file);
	}

	public static CharacterSource FromFile(FileSystemInfo file)
	{
		var text = System.IO.File.ReadAllText(file.FullName);

		return new CharacterSource(text) { File = file };
	}

	public static async Task<CharacterSource> FromFileAsync(FileSystemInfo file,
		CancellationToken cancellationToken = default)
	{
		var text = await System.IO.File.ReadAllTextAsync(file.FullName, cancellationToken);

		return new CharacterSource(text) { File = file };
	}

	public bool TryConsume(out char character)
	{
		var success = charQueue.TryDequeue(out character);
		if (!success)
		{
			return false;
		}

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

		return character != default;
	}

	public bool TryPeek(out char character)
	{
		return TryPeek(0, out character);
	}

	public IEnumerable<char> ConsumeUntil(char c)
	{
		var characters = new List<char>();

		while (TryPeek(out var nextChar))
		{
			if (nextChar == c)
			{
				break;
			}

			_ = TryConsume(out var character);

			characters.Add(character);
		}

		return characters;
	}
}
