using StepLang.Tooling.Formatting.Fixers;

namespace StepLang.Tooling.Formatting.FixerSets;

public class DefaultFixerSet : BaseFixerSet
{
    public override IEnumerable<IFixer> GetFixers()
    {
        yield return new FileEncodingFixer();
        yield return new LineEndingFixer();
        yield return new TrailingWhitespaceFixer();
        yield return new IndentationFixer();
    }
}