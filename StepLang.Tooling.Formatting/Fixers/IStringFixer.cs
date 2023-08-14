using StepLang.Tooling.Formatting.Fixers.Results;

namespace StepLang.Tooling.Formatting.Fixers;

public interface IStringFixer : IFixer
{
    /// <summary>
    /// Applies a fix to the input string.
    /// </summary>
    /// <param name="input">A string to apply the fix to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A result containing a string with the applied fix.</returns>
    Task<StringFixResult> FixAsync(string input, CancellationToken cancellationToken = default);
}