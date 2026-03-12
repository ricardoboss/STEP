using StepLang.Expressions.Results;

namespace StepLang.Tests.Expressions.Results;

public class NumberResultTest
{
	[Theory]
	[TestCase(123d, "123")]
	[TestCase(123.321, "123.321")]
	[TestCase(0d, "0")]
	[TestCase(0.1, "0.1")]
	[TestCase(-1.234, "-1.234")]
	[TestCase(double.MaxValue, "1.7976931348623157E+308")]
	public void TestDoubleToString(double value, string expected)
	{
		var result = new NumberResult(value);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.ToString(), Is.EqualTo(expected));
			Assert.That((string)result, Is.EqualTo(expected));
		}
	}
}
