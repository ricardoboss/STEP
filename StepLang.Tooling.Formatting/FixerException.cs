using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting;

public class FixerException : Exception
{
    public IAnalyzer Analyzer { get; }

    public FixerException(IAnalyzer analyzer, string? message = null, Exception? inner = null) : base(message, inner)
    {
        Analyzer = analyzer;
    }
}