using StepLang.Framework;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tests.Framework;

public class GraphemeExtensionsTest
{
	[TestCase("", 0, null)]
	[TestCase("", -1, null)]
	[TestCase("", 1, null)]
	[TestCase("a", 0, "a")]
	[TestCase("a", -1, null)]
	[TestCase("a", 1, null)]
	[TestCase("abc", 1, "b")]
	[TestCase("a🤷‍♂️b", 0, "a")]
	[TestCase("a🤷‍♂️b", 1, "🤷‍♂️")]
	[TestCase("🤷‍♂️ab", 0, "🤷‍♂️")]
	[TestCase("🤷‍♂️ab", 1, "a")]
	[TestCase("a🤷‍♂️b", 2, "b")]
	public void TestGraphemeAt(string str, int index, string? expected)
	{
		var actual = str.GraphemeAt(index);

		Assert.That(actual, Is.EqualTo(expected));
	}

	[TestCase("", 0)]
	[TestCase("a", 1)]
	[TestCase("abc", 3)]
	[TestCase("a🤷‍♂️b", 3)]
	public void TestGraphemeLength(string str, int expected)
	{
		var actual = str.GraphemeLength();

		Assert.That(actual, Is.EqualTo(expected));
	}

	[TestCase("", 0, 0, "")]
	[TestCase("a", 0, 1, "a")]
	[TestCase("a", 0, 2, "a")]
	[TestCase("a", 1, 1, "")]
	[TestCase("abc", 1, 2, "bc")]
	[TestCase("a🤷‍♂️b", 0, 1, "a")]
	[TestCase("a🤷‍♂️b", 1, 1, "🤷‍♂️")]
	[TestCase("a🤷‍♂️b", 1, 2, "🤷‍♂️b")]
	[TestCase("a🤷‍♂️b", 0, 2, "a🤷‍♂️")]
	public void TestGraphemeSubstring(string str, int start, int length, string expected)
	{
		var actual = str.GraphemeSubstring(start, length);

		Assert.That(actual, Is.EqualTo(expected));
	}

	[TestCase("abc", "", new[]
	{
				"a", "b",
				"c",
		})]
	[TestCase("a,b,c", ",", new[]
{
		"a", "b",
		"c",
	})]
	[TestCase("a and b and c", " and ", new[]
{
		"a", "b",
		"c",
	})]
	[SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments",
	Justification = "Array is only created once")]
	public void TestGraphemeSplit(string str, string separator, IEnumerable<string> expected)
	{
		var actual = str.GraphemeSplit(separator);

		Assert.That(actual, Is.EqualTo(expected));
	}
}
