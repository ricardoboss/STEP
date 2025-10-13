using StepLang.Diagnostics;

namespace StepLang.Tests.Diagnostics;

public static class VariableDeclarationCollectorTest
{
	[Fact]
	public static void TestFindsSimpleDeclaration()
	{
		const string sourceCode = "string iamunused = \"that's right\"";

		var documentUri = new Uri("file:///path/to/file.step");
		var root = sourceCode.AsParsed();

		var collector = new VariableDeclarationCollector(documentUri);
		collector.Visit(root);

		var declaration = Assert.Single(collector.Declarations);
		Assert.Equal("iamunused", declaration.Name);
	}
}
