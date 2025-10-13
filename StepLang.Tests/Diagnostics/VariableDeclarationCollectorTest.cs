using StepLang.Diagnostics;

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
	}
}
