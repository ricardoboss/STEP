using StepLang.Expressions.Results;

namespace StepLang.Tests.Expressions.Results;

public class StringResultTest
{
	[Theory]
	[TestCase("value", "\"value\"")]
	[TestCase("\n", "\"\\n\"")]
	public void StringResultToString(string value, string expected)
	{
		var result = new StringResult(value);

		Assert.That(result.ToString(), Is.EqualTo(expected));
	}
}
