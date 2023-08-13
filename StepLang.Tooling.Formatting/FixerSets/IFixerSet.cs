using StepLang.Formatters.Fixers;

namespace StepLang.Formatters.FixerSets;

public interface IFixerSet : IEnumerable<IFixer>
{
    public IEnumerable<IFixer> GetFixers();
}