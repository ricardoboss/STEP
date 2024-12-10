using StepLang.Parsing;
using StepLang.Tokenizing;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tests.Parsing;

[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes")]
public class TokenQueueTest
{
	[Fact]
	public void TestDequeueThrowsIfEmpty()
	{
		var queue = new TokenQueue();

		_ = Assert.Throws<UnexpectedEndOfTokensException>(() => queue.Dequeue());
		_ = Assert.Throws<UnexpectedEndOfTokensException>(() => queue.Dequeue(1));
		_ = Assert.Throws<UnexpectedEndOfTokensException>(() => queue.Dequeue(TokenType.Whitespace));
	}
}
