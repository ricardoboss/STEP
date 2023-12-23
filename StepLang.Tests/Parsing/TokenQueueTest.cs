using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Tests.Parsing;

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