using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting.AnalyzerSet;

public class DefaultAnalyzerSet : BaseAnalyzerSet
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
