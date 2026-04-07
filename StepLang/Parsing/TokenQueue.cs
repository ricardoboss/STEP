using StepLang.Tokenizing;
using StepLang.Utils;
using System.Diagnostics.CodeAnalysis;

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
		if (tokenList.First is null)
		{
			return false;
		}

		var node = tokenList.First;
		while (node is not null)
		{
			if (!IgnoreMeaningless || node.Value.Type.HasMeaning())
			{
				token = node.Value;

				// Remove all nodes from the front up to and including this one
				while (tokenList.First != node)
				{
					tokenList.RemoveFirst();
				}

				tokenList.RemoveFirst();

				LastToken = token;
				return true;
			}

			node = node.Next;
		}

		return false;
	}

	public Result<Token> Dequeue()
	{
		if (TryDequeue(out var token))
			return token.ToResult();

		var exception = new UnexpectedEndOfTokensException(LastToken);

		return exception.ToErr<Token>();
	}

	public Result<Token[]> Dequeue(int count)
	{
		var tokens = new Token[count];

		for (var i = 0; i < count; i++)
		{
			var result = Dequeue();
			if (result is Ok<Token> { Value: var token })
				tokens[i] = token;
			else
				return result.Map<Token, Token[]>();
		}

		return tokens.ToResult();
	}

	public Result<Token> Dequeue(params TokenType[] allowed)
	{
		Token? token;
		do
		{
			if (TryDequeue(out token))
			{
				continue;
			}

			return new UnexpectedEndOfTokensException(LastToken).ToErr<Token>();
		} while (!token.Type.HasMeaning() && IgnoreMeaningless);

		if (allowed.Contains(token.Type))
			return token.ToResult();

		return new UnexpectedTokenException(token, allowed).ToErr<Token>();
	}

	public TokenType? PeekType(int offset = 0)
	{
		var token = Peek(offset);

		return token?.Type;
	}

	public Token? Peek(int offset = 0)
	{
		var node = tokenList.First;
		var remaining = offset;

		while (node is not null)
		{
			if (!IgnoreMeaningless || node.Value.Type.HasMeaning())
			{
				if (remaining == 0)
				{
					return node.Value;
				}

				remaining--;
			}

			node = node.Next;
		}

		return null;
	}
}
