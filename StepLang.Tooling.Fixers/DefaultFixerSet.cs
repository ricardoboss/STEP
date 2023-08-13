namespace StepLang.Formatters;

public class DefaultFixerSet : BaseFixerSet
{
    public override IEnumerable<IFixer> GetFixers()
    {
        yield return new FileEncodingFixer();
        yield return new LineEndingFixer();
        yield return new TrailingWhitespaceFixer();
    }
}