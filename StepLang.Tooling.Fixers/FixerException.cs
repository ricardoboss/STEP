namespace StepLang.Formatters;

public class FixerException : Exception
{
    public IFixer Fixer { get; }

    public FixerException(IFixer fixer, string? message) : base(message)
    {
        Fixer = fixer;
    }
}