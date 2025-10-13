using StepLang.Expressions.Results;
using StepLang.Framework.Pure;

namespace StepLang.Tests.Framework.Pure;

public class IndexOfFunctionTest
{
	[TestCase("", "", -1)]
	[TestCase("", "a", -1)]
	[TestCase("a", "", 0)]
	[TestCase("a", "a", 0)]
	[TestCase("a", "b", -1)]
	[TestCase("ab", "a", 0)]
	[TestCase("ab", "b", 1)]
	[TestCase("a🤷‍♂️b", "a", 0)]
	[TestCase("a🤷‍♂️b", "🤷‍♂️", 1)]
	[TestCase("a🤷‍♂️b", "b", 2)]
	public void TestIndexOfString(string subject, string value, int expected)
	{
		var result = IndexOfFunction.GetResult(new StringResult(subject), new StringResult(value));

		Assert.That(result, Is.TypeOf<NumberResult>());
		var numResult = (NumberResult)result;
		Assert.That(numResult.Value, Is.EqualTo(expected));
	}
}
