using StepLang.Tokenizing;

namespace StepLang.Tests.Tokenizing;

public static class TokenTest
{
	[Theory]
	[TestCase("\"\"", "")]
	[TestCase("\"hello\"", "hello")]
	[TestCase("\"quote: \\\"message\\\"\"", "quote: \"message\"")]
	public static void TestStringValueIsCorrect(string value, string stringValue)
	{
		var token = new Token(TokenType.LiteralString, value);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(token.Value, Is.EqualTo(value));
			Assert.That(token.StringValue, Is.EqualTo(stringValue));
		}
	}
}
