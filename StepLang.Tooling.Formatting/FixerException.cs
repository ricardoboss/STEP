using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting;

/// <summary>
/// The base class for all exceptions thrown by fixers.
/// </summary>
public class FixerException : Exception
{
    /// <summary>
    /// The analyzer that caused the exception.
    /// </summary>
    public IAnalyzer Analyzer { get; }

    /// <summary>
    /// Creates a new <see cref="FixerException"/>.
    /// </summary>
    /// <param name="analyzer">The analyzer that caused the exception.</param>
    /// <param name="message">The message of the exception.</param>
    /// <param name="inner">The inner exception.</param>
    public FixerException(IAnalyzer analyzer, string? message = null, Exception? inner = null) : base(message, inner)
    {
        Analyzer = analyzer;
    }
}