using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Tests.Interpreting;

public class ScopeTest
{
    [Fact]
    public void TestThrowsForUndefinedIdentifiers()
    {
        var scope = Scope.GlobalScope;

        Assert.Throws<UndefinedIdentifierException>(() => scope.GetVariable(new(TokenType.Identifier, "undefined")));
    }
}