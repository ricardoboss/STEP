using StepLang.Tooling.Analysis.AnalyzerSet;

namespace StepLang.Tooling.Analysis.Tests.AnalyzerSet;

public static class DefaultAnalyzerSetTest
{
	[Fact]
	public static void TestAllAnalyzersHaveUniqueNonEmptyNames()
	{
		var uniqueNames = new HashSet<string>();
		foreach (var analyzer in new DefaultAnalyzerSet())
		{
			Assert.NotEmpty(analyzer.Name);
			Assert.True(uniqueNames.Add(analyzer.Name));
		}
	}
}
