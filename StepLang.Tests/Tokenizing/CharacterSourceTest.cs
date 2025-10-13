using StepLang.Tokenizing;

namespace StepLang.Tests.Tokenizing;

public class CharacterSourceTest
{
	[Test]
	public void TestLocation()
	{
		var source = new CharacterSource("a\nb");

		Assert.That(source.Line, Is.EqualTo(1));
		Assert.That(source.Column, Is.EqualTo(1));

		Assert.That(source.TryConsume(out var first), Is.True);
		Assert.That(first, Is.EqualTo('a'));

		Assert.That(source.Line, Is.EqualTo(1));
		Assert.That(source.Column, Is.EqualTo(2));

		Assert.That(source.TryConsume(out var second), Is.True);
		Assert.That(second, Is.EqualTo('\n'));

		Assert.That(source.Line, Is.EqualTo(2));
		Assert.That(source.Column, Is.EqualTo(1));

		Assert.That(source.TryConsume(out var third), Is.True);
		Assert.That(third, Is.EqualTo('b'));

		Assert.That(source.Line, Is.EqualTo(2));
		Assert.That(source.Column, Is.EqualTo(2));
	}
}
