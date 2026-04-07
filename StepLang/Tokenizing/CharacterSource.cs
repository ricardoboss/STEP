using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates")]
public class CharacterSource
{
	private readonly char[] chars;
	private int position;

	public CharacterSource(IEnumerable<char> source)
	{
		chars = source is char[] arr ? arr : source.ToArray();
	}

	private CharacterSource(char[] chars)
	{
		this.chars = chars;
	}

	public Uri? DocumentUri { get; init; }

	public int Column { get; private set; } = 1;

	public int Line { get; private set; } = 1;

	public static implicit operator CharacterSource(string source) => new(source.ToCharArray());

	public static implicit operator CharacterSource(FileSystemInfo file) => FromFile(file);

	public static CharacterSource FromFile(FileSystemInfo file)
	{
		var text = File.ReadAllText(file.FullName);

		return new CharacterSource(text.ToCharArray()) { DocumentUri = new Uri(file.FullName) };
	}

	public static async Task<CharacterSource> FromFileAsync(FileSystemInfo file,
		CancellationToken cancellationToken = default)
	{
		var text = await File.ReadAllTextAsync(file.FullName, cancellationToken);

		return new CharacterSource(text.ToCharArray()) { DocumentUri = new Uri(file.FullName) };
	}

	public bool TryConsume(out char character)
	{
		if (position >= chars.Length)
		{
			character = default;
			return false;
		}

		character = chars[position++];

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

	public bool TryPeek(out char character)
	{
		if (position >= chars.Length)
		{
			character = default;
			return false;
		}

		character = chars[position];
		return character != 0;
	}

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
