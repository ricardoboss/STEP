using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Tests.Interpreting;

public class ScopeTest
{
	[Fact]
	public void TestThrowsForUndefinedIdentifiers()
	{
		var scope = Scope.GlobalScope;

		var e = Assert.Throws<UndefinedIdentifierException>(() =>
			scope.GetVariable(new Token(TokenType.Identifier, "undefined")));

		Assert.Equal("Variable 'undefined' was not declared (used at 1:1)", e.Message);
	}
}
