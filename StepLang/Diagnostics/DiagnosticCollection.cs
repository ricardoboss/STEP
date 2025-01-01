using StepLang.Tokenizing;
using System.Collections;

namespace StepLang.Diagnostics;

public class DiagnosticCollection : ICollection<Diagnostic>
{
	private readonly List<Diagnostic> diagnostics = [];

	public void Add(Diagnostic diagnostic) => diagnostics.Add(diagnostic);

	public void Add(DiagnosticArea area, Severity severity, string message, string code, Token? token = null, TokenLocation? location = null,
		DiagnosticKind? kind = null, IEnumerable<Token>? relatedTokens = null)
	{
		diagnostics.Add(new Diagnostic
		{
			Area = area,
			Severity = severity,
			Message = message,
			Code = code,
			Kind = kind,
			Token = token,
			Location = location,
			RelatedTokens = relatedTokens,
		});
	}

	public void Clear() => diagnostics.Clear();

	public bool Contains(Diagnostic item) => diagnostics.Contains(item);

	public bool ContainsErrors => diagnostics.Any(d => d.Severity == Severity.Error);

	public void CopyTo(Diagnostic[] array, int arrayIndex) => diagnostics.CopyTo(array, arrayIndex);

	public bool Remove(Diagnostic item) => diagnostics.Remove(item);

	public int Count => diagnostics.Count;

	public bool IsReadOnly => false;

	public IEnumerator<Diagnostic> GetEnumerator() => diagnostics.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
