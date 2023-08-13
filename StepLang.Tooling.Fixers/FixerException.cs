namespace StepLang.Formatters;

public class FixerException : Exception
{
    public IFixer Fixer { get; }

    public FixerException(IFixer fixer, string? message = null, Exception? inner = null) : base(message, inner)
    {
        Fixer = fixer;
    }
}