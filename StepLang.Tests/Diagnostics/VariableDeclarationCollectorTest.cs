using StepLang.Diagnostics;
using StepLang.Tokenizing;

namespace StepLang.Tests.Diagnostics;

public static class VariableDeclarationCollectorTest
{
	[Test]
	public static void TestFindsSimpleDeclaration()
	{
		const string sourceCode = "string iamunused = \"that's right\"";

		var documentUri = new Uri("file:///path/to/file.step");
		var root = sourceCode.AsParsed();

		var collector = new VariableDeclarationCollector(documentUri);
		collector.Visit(root);

		Assert.That(collector.Declarations, Has.Count.EqualTo(1));
		Assert.That(collector.Declarations[0].Name, Is.EqualTo("iamunused"));
		Assert.That(collector.Scopes, Has.Count.EqualTo(1));
		Assert.That(collector.Scopes[0].Usages, Is.Empty);
	}

	[Test]
	public static void TestFindsSimpleUsageAsParam()
	{
		const string sourceCode = """
		                          string foo = "hello"
		                          println(foo)
		                          """;

		var documentUri = new Uri("file:///path/to/file.step");
		var root = sourceCode.AsParsed();

		var collector = new VariableDeclarationCollector(documentUri);
		collector.Visit(root);

		Assert.That(collector.Declarations, Has.Count.EqualTo(1));
		Assert.That(collector.Declarations[0].Name, Is.EqualTo("foo"));
		Assert.That(collector.Scopes, Has.Count.EqualTo(1));
		Assert.That(collector.Scopes[0].Usages, Contains.Key("foo"));

		var usage = collector.Scopes[0].Usages["foo"];
		Assert.That(usage, Has.Count.EqualTo(1));
		Assert.That(usage[0].Type, Is.EqualTo(TokenType.Identifier));
		Assert.That(usage[0].Value, Is.EqualTo("foo"));
	}
}
