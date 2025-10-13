using StepLang.Tooling.Analysis.AnalyzerSet;

namespace StepLang.Tooling.Analysis.Tests.AnalyzerSet;

public static class DefaultAnalyzerSetTest
{
	[Test]
	public static void TestAllAnalyzersHaveUniqueNonEmptyNames()
	{
		var uniqueNames = new HashSet<string>();
		foreach (var analyzer in new DefaultAnalyzerSet())
		{
			Assert.That(analyzer.Name, Is.Not.Empty);
			Assert.That(uniqueNames.Add(analyzer.Name), Is.True);
		}
	}
}
