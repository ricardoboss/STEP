using System.Collections;

namespace StepLang.Formatters;

public abstract class BaseFixerSet : IFixerSet
{
    public IEnumerator<IFixer> GetEnumerator() => GetFixers().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public abstract IEnumerable<IFixer> GetFixers();
}