using StepLang.Tooling.Analysis.Analyzers;
using System.Collections;

namespace StepLang.Tooling.Analysis.AnalyzerSet;

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
