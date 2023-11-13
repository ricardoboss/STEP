namespace StepLang.Tooling.Highlighting.Tests;

public class ColorSchemeTest
{
    [Theory]
    [InlineData("pale")]
    [InlineData("PALE")]
    [InlineData("dim")]
    [InlineData("mono")]
    public void TestKnownColorSchemeNames(string name)
    {
        var scheme = ColorScheme.ByName(name);

        Assert.NotNull(scheme);
    }

    [Fact]
    public void TestNamesContainsOnlyKnownSchemes()
    {
        foreach (var name in ColorScheme.Names)
        {
            var scheme = ColorScheme.ByName(name);

            Assert.NotNull(scheme);
        }
    }

    [Fact]
    public void TestThrowsForUnknownNames()
    {
        Assert.Throws<NotSupportedException>(() =>
        {
            _ = ColorScheme.ByName("unknown");
        });
    }
}
