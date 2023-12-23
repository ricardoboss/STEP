using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting.AnalyzerSet;

/// <summary>
/// A set of default analyzers.
/// </summary>
/// <seealso cref="FileEncodingAnalyzer"/>
/// <seealso cref="LineEndingAnalyzer"/>
/// <seealso cref="TrailingWhitespaceAnalyzer"/>
/// <seealso cref="IndentationAnalyzer"/>
/// <seealso cref="KeywordCasingAnalyzer"/>
/// <seealso cref="TypeNameCasingAnalyzer"/>
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