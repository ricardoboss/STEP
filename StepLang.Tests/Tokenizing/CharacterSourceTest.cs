using StepLang.Tokenizing;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tests.Tokenizing;

[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes")]
public class CharacterSourceTest
{
	[Fact]
	public void TestLocation()
	{
		var source = new CharacterSource("a\nb");

		Assert.Equal(1, source.Line);
		Assert.Equal(1, source.Column);

		Assert.True(source.TryConsume(out var first));
		Assert.Equal('a', first);

		Assert.Equal(1, source.Line);
		Assert.Equal(2, source.Column);

		Assert.True(source.TryConsume(out var second));
		Assert.Equal('\n', second);

		Assert.Equal(2, source.Line);
		Assert.Equal(1, source.Column);

		Assert.True(source.TryConsume(out var third));
		Assert.Equal('b', third);

		Assert.Equal(2, source.Line);
		Assert.Equal(2, source.Column);
	}
}
