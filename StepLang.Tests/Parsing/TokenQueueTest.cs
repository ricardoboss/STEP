using StepLang.Parsing;
using StepLang.Tokenizing;
using StepLang.Utils;

namespace StepLang.Tests.Parsing;

public class TokenQueueTest
{
	[Test]
	public void TestDequeueReturnsErrorIfEmpty()
	{
		var queue = new TokenQueue();

		var noParamsResult = queue.Dequeue();
		var noParamsErr = AssertIsType<Err<Token>>(noParamsResult);
		_ = AssertIsType<UnexpectedEndOfTokensException>(noParamsErr.Exception);

		var countParamResult = queue.Dequeue(1);
		var countParamErr = AssertIsType<Err<Token[]>>(countParamResult);
		_ = AssertIsType<UnexpectedEndOfTokensException>(countParamErr.Exception);

		var allowedTypesParamResult = queue.Dequeue(TokenType.Whitespace);
		var allowedTypesParamErr = AssertIsType<Err<Token>>(allowedTypesParamResult);
		_ = AssertIsType<UnexpectedEndOfTokensException>(allowedTypesParamErr.Exception);
	}

	private static T AssertIsType<T>(object? value)
	{
		Assert.That(value, Is.TypeOf<T>());
		return (T)value!;
	}
}
