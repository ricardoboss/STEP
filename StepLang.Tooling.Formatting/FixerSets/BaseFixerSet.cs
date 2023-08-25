using System.Collections;
using StepLang.Tooling.Formatting.Fixers;

namespace StepLang.Tooling.Formatting.FixerSets;

public abstract class BaseFixerSet : IFixerSet
{
    public IEnumerator<IFixer> GetEnumerator() => GetFixers().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public abstract IEnumerable<IFixer> GetFixers();
}