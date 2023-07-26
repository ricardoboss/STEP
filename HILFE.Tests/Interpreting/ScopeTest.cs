using HILFE.Interpreting;

namespace HILFE.Tests.Interpreting;

public class ScopeTest
{
    [Fact]
    public void TestThrowsForUndefinedIdentifiers()
    {
        var scope = Scope.GlobalScope;

        Assert.Throws<UndefinedIdentifierException>(() => scope.GetByIdentifier("undefined"));
    }
}