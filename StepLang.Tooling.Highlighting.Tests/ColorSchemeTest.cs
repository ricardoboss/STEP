namespace StepLang.Tooling.Highlighting.Tests;

public class ColorSchemeTest
{
	[TestCase("pale")]
	[TestCase("PALE")]
	[TestCase("dim")]
	[TestCase("mono")]
	public void TestKnownColorSchemeNames(string name)
	{
		var scheme = ColorScheme.ByName(name);

		Assert.That(scheme, Is.Not.Null);
	}

	[Test]
	public void TestNamesContainsOnlyKnownSchemes()
	{
		foreach (var name in ColorScheme.Names)
		{
			var scheme = ColorScheme.ByName(name);

			Assert.That(scheme, Is.Not.Null);
		}
	}

	[Test]
	public void TestThrowsForUnknownNames()
	{
		var e = Assert.Throws<NotSupportedException>(() => { _ = ColorScheme.ByName("unknown"); });

		Assert.That(e.Message, Is.EqualTo("The color scheme 'unknown' is not supported."));
	}
}
