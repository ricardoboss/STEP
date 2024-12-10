using StepLang.Tooling.Formatting.Analyzers;
using System.Collections;

namespace StepLang.Tooling.Formatting.AnalyzerSet;

public abstract class BaseAnalyzerSet : IAnalyzerSet
{
	/// <inheritdoc />
	public abstract IEnumerator<IAnalyzer> GetEnumerator();

	/// <inheritdoc />
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
