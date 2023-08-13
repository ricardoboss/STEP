using System.Collections;
using StepLang.Formatters.Fixers;

namespace StepLang.Formatters.FixerSets;

public abstract class BaseFixerSet : IFixerSet
{
    public IEnumerator<IFixer> GetEnumerator() => GetFixers().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public abstract IEnumerable<IFixer> GetFixers();
}