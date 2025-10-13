using StepLang.Tokenizing;

namespace StepLang.Tests.Tokenizing;

public class IdentifierTests
{
	[TestCase("a")]
	[TestCase("A")]
	[TestCase("a1")]
	[TestCase("a_1")]
	[TestCase("_23")]
	[TestCase("ThisIsAValidIdentifier")]
	[TestCase("This_Is_Also_Valid")]
	[TestCase("This_Is_123")]
	[TestCase("This_Is_123_")]
	public void TestValidIdentifiers(string identifier)
	{
		Assert.That(identifier.IsValidIdentifier(), Is.True);
	}

	[TestCase("")]
	[TestCase(" ")]
	[TestCase("a ")]
	[TestCase(" a")]
	[TestCase("\0")]
	[TestCase("\u0001")]
	[TestCase("1a")]
	[TestCase("1_")]
	[TestCase("1a_")]
	[TestCase("1_2")]
	[TestCase("1_2a")]
	[TestCase("1_2a_")]
	[TestCase("a.b")]
	[TestCase("a b")]
	[TestCase("#")]
	[TestCase("$")]
	[TestCase("a$b")]
	[TestCase("a#b")]
	[TestCase("$0b")]
	[TestCase("#0b")]
	[TestCase("a0b$")]
	[TestCase("a0b#")]
	[TestCase("ï")]
	public void TestInvalidIdentifiers(string identifier)
	{
		Assert.That(identifier.IsValidIdentifier(), Is.False);
	}
}
