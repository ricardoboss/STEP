using System.Diagnostics.CodeAnalysis;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix")]
public class TokenQueue
{
	private readonly LinkedList<Token> tokenList;

	public TokenQueue()
	{
		tokenList = [];
	}

	public TokenQueue(IEnumerable<Token> tokens)
	{
		tokenList = new LinkedList<Token>(tokens);
	}

	public bool IgnoreMeaningless { get; set; }

	public Token? LastToken { get; private set; }

	public bool TryDequeue([NotNullWhen(true)] out Token? token)
	{
		token = null;
		if (tokenList.Count == 0)
		{
			return false;
		}

		var skip = 0;
		do
		{
			token = tokenList.Skip(skip).FirstOrDefault();
			skip++;
		} while (IgnoreMeaningless && !(token?.Type.HasMeaning() ?? true));

		if (token is null)
		{
			return false;
		}

		LastToken = token;

		for (; skip > 0; skip--)
		{
			tokenList.RemoveFirst();
		}

		return true;
	}

	public Token Dequeue()
	{
		if (!TryDequeue(out var token))
		{
			throw new UnexpectedEndOfTokensException(LastToken?.Location);
		}

		return token;
	}

	public Token[] Dequeue(int count)
	{
		var tokens = new Token[count];

		for (var i = 0; i < count; i++)
		{
			tokens[i] = Dequeue();
		}

		return tokens;
	}

	public Token Peek(int offset = 0)
	{
		var source = tokenList.AsEnumerable();
		if (IgnoreMeaningless)
		{
			source = source.Where(t => t.Type.HasMeaning());
		}

		return source.Skip(offset).First();
	}

	public TokenType PeekType(int offset = 0)
	{
		var token = Peek(offset);

		return token.Type;
	}

	public Token Dequeue(params TokenType[] allowed)
	{
		Token? token;
		do
		{
			if (TryDequeue(out token))
			{
				continue;
			}

			var typeInfo = allowed.Length switch
			{
				0 => "any token",
				1 => $"a {allowed[0].ToDisplay()}",
				_ => $"any one of {string.Join(',', allowed.Select(TokenTypes.ToDisplay))}",
			};

			throw new UnexpectedEndOfTokensException(LastToken?.Location, $"Expected {typeInfo}");
		} while (!token.Type.HasMeaning() && IgnoreMeaningless);

		if (!allowed.Contains(token.Type))
		{
			throw new UnexpectedTokenException(token, allowed);
		}

		return token;
	}
}
