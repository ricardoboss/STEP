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
        var e = Assert.Throws<NotSupportedException>(() =>
        {
            _ = ColorScheme.ByName("unknown");
        });

        Assert.Equal("The color scheme 'unknown' is not supported.", e.Message);
    }
}
