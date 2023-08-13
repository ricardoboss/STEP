namespace StepLang.Formatters;

public interface IFixerSet : IEnumerable<IFixer>
{
    public IEnumerable<IFixer> GetFixers();
}