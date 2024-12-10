using StepLang.Framework;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tests.Framework;

[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes")]
public class GraphemeExtensionsTest
{
	[Theory]
	[InlineData("", 0, null)]
	[InlineData("", -1, null)]
	[InlineData("", 1, null)]
	[InlineData("a", 0, "a")]
	[InlineData("a", -1, null)]
	[InlineData("a", 1, null)]
	[InlineData("abc", 1, "b")]
	[InlineData("a🤷‍♂️b", 0, "a")]
	[InlineData("a🤷‍♂️b", 1, "🤷‍♂️")]
	[InlineData("🤷‍♂️ab", 0, "🤷‍♂️")]
	[InlineData("🤷‍♂️ab", 1, "a")]
	[InlineData("a🤷‍♂️b", 2, "b")]
	public void TestGraphemeAt(string str, int index, string? expected)
	{
		var actual = str.GraphemeAt(index);

		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("", 0)]
	[InlineData("a", 1)]
	[InlineData("abc", 3)]
	[InlineData("a🤷‍♂️b", 3)]
	public void TestGraphemeLength(string str, int expected)
	{
		var actual = str.GraphemeLength();

		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("", 0, 0, "")]
	[InlineData("a", 0, 1, "a")]
	[InlineData("a", 0, 2, "a")]
	[InlineData("a", 1, 1, "")]
	[InlineData("abc", 1, 2, "bc")]
	[InlineData("a🤷‍♂️b", 0, 1, "a")]
	[InlineData("a🤷‍♂️b", 1, 1, "🤷‍♂️")]
	[InlineData("a🤷‍♂️b", 1, 2, "🤷‍♂️b")]
	[InlineData("a🤷‍♂️b", 0, 2, "a🤷‍♂️")]
	public void TestGraphemeSubstring(string str, int start, int length, string expected)
	{
		var actual = str.GraphemeSubstring(start, length);

		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("abc", "", new[]
	{
		"a", "b",
		"c",
	})]
	[InlineData("a,b,c", ",", new[]
	{
		"a", "b",
		"c",
	})]
	[InlineData("a and b and c", " and ", new[]
	{
		"a", "b",
		"c",
	})]
	[SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments",
		Justification = "Array is only created once")]
	public void TestGraphemeSplit(string str, string separator, IEnumerable<string> expected)
	{
		var actual = str.GraphemeSplit(separator);

		Assert.Equal(expected, actual);
	}
}
