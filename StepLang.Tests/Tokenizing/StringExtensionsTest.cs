using StepLang.Tokenizing;

namespace StepLang.Tests.Tokenizing;

public static class StringExtensionsTest
{
	[Theory]
	[TestCase(null, null)]
	[TestCase("", "")]
	[TestCase("a", "a")]
	[TestCase("\\", @"\\")]
	[TestCase("\"", @"\""")]
	[TestCase("'", @"\'")]
	[TestCase("\n", @"\n")]
	[TestCase("\r", @"\r")]
	[TestCase("\t", @"\t")]
	[TestCase("\x1F", @"\x1F")]
	[TestCase("\x7F", @"\x7F")]
	public static void TestEscapeAndUnescapeControlChars(string? value, string? expectedEscapedResult)
	{
		var escapedResult = value.EscapeControlCharacters();

		Assert.That(escapedResult, Is.EqualTo(expectedEscapedResult));

		// test the inverse behaves the same way
		var unescapedResult = escapedResult.UnescapeControlChars();

		Assert.That(unescapedResult, Is.EqualTo(value));
	}

	[Test]
	public static void TestUnescapeControlCharsIsCaseInsensitive()
	{
		const string lowerCase = "\\x1f";
		const string upperCase = "\\X1F";

		using (Assert.EnterMultipleScope())
		{
			Assert.That(lowerCase.UnescapeControlChars(), Is.EqualTo("\x1F"));
			Assert.That(upperCase.UnescapeControlChars(), Is.EqualTo("\x1F"));
		}
	}
}
