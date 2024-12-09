using StepLang.Tokenizing;

namespace StepLang.Tests.Tokenizing;

public class IdentifierTests
{
	[Theory]
	[InlineData("a")]
	[InlineData("A")]
	[InlineData("a1")]
	[InlineData("a_1")]
	[InlineData("_23")]
	[InlineData("ThisIsAValidIdentifier")]
	[InlineData("This_Is_Also_Valid")]
	[InlineData("This_Is_123")]
	[InlineData("This_Is_123_")]
	public void TestValidIdentifiers(string identifier)
	{
		Assert.True(identifier.IsValidIdentifier());
	}

	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData("a ")]
	[InlineData(" a")]
	[InlineData("\0")]
	[InlineData("\u0001")]
	[InlineData("1a")]
	[InlineData("1_")]
	[InlineData("1a_")]
	[InlineData("1_2")]
	[InlineData("1_2a")]
	[InlineData("1_2a_")]
	[InlineData("a.b")]
	[InlineData("a b")]
	[InlineData("#")]
	[InlineData("$")]
	[InlineData("a$b")]
	[InlineData("a#b")]
	[InlineData("$0b")]
	[InlineData("#0b")]
	[InlineData("a0b$")]
	[InlineData("a0b#")]
	[InlineData("ï")]
	public void TestInvalidIdentifiers(string identifier)
	{
		Assert.False(identifier.IsValidIdentifier());
	}
}
