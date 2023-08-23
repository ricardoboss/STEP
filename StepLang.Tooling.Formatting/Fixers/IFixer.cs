namespace StepLang.Tooling.Formatting.Fixers;

/// <summary>
/// This is just a marker for all fixers.
/// </summary>
public interface IFixer
{
    public string Name => GetType().Name;
}