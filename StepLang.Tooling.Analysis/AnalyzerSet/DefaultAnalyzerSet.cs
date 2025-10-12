using StepLang.Tooling.Analysis.Analyzers;

namespace StepLang.Tooling.Analysis.AnalyzerSet;

public sealed class DefaultAnalyzerSet : BaseAnalyzerSet
{
	/// <inheritdoc />
	public override IEnumerator<IAnalyzer> GetEnumerator()
	{
		yield return new FileEncodingAnalyzer();
		yield return new LineEndingAnalyzer();
		yield return new TrailingWhitespaceAnalyzer();
		yield return new IndentationAnalyzer();
		yield return new KeywordCasingAnalyzer();
		yield return new TypeNameCasingAnalyzer();
	}
}
