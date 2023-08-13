using StepLang.Tooling.Formatting.Fixers;

namespace StepLang.Tooling.Formatting.FixerSets;

public interface IFixerSet : IEnumerable<IFixer>
{
    public IEnumerable<IFixer> GetFixers();
}