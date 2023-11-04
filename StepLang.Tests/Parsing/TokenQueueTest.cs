using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Tests.Parsing;

public class TokenQueueTest
{
    [Fact]
    public void TestDequeueThrowsIfEmpty()
    {
        var queue = new TokenQueue();

        Assert.Throws<UnexpectedEndOfTokensException>(() => queue.Dequeue());
        Assert.Throws<UnexpectedEndOfTokensException>(() => queue.Dequeue(1));
        Assert.Throws<UnexpectedEndOfTokensException>(() => queue.Dequeue(TokenType.Whitespace));
    }
}