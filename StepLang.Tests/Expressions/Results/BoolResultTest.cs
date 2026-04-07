using StepLang.Expressions.Results;

namespace StepLang.Tests.Expressions.Results;

public class BoolResultTest
{
	[TestCase(true, true, true)]
	[TestCase(true, false, false)]
	[TestCase(false, true, false)]
	[TestCase(false, false, true)]
	public void TestEquality(bool left, bool right, bool expected)
	{
		BoolResult leftResult = left;
		BoolResult rightResult = right;

		var result = leftResult == rightResult;

		Assert.That(result.Value, Is.EqualTo(expected));
	}

	[TestCase(true, true, false)]
	[TestCase(true, false, true)]
	[TestCase(false, true, true)]
	[TestCase(false, false, false)]
	public void TestInequality(bool left, bool right, bool expected)
	{
		BoolResult leftResult = left;
		BoolResult rightResult = right;

		var result = leftResult != rightResult;

		Assert.That(result.Value, Is.EqualTo(expected));
	}
}
