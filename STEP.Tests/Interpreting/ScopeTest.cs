using STEP.Interpreting;

namespace STEP.Tests.Interpreting;

public class ScopeTest
{
    [Fact]
    public void TestThrowsForUndefinedIdentifiers()
    {
        var scope = Scope.GlobalScope;

        Assert.Throws<UndefinedIdentifierException>(() => scope.GetVariable("undefined"));
    }
}