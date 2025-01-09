using StepLang.Tokenizing;
using System.Collections;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace StepLang.Diagnostics;

public class DiagnosticCollectionChangedEventArgs(
	NotifyCollectionChangedAction action,
	IReadOnlyList<Diagnostic>? newItems,
	IReadOnlyList<Diagnostic>? oldItems
) : EventArgs
{
	public NotifyCollectionChangedAction Action { get; } = action;

	public IReadOnlyList<Diagnostic>? NewItems { get; } = newItems;

	public IReadOnlyList<Diagnostic>? OldItems { get; } = oldItems;
}

public class DiagnosticCollection : ICollection<Diagnostic>
{
	private readonly ObservableCollection<Diagnostic> diagnostics = [];

	public event EventHandler<DiagnosticCollectionChangedEventArgs>? CollectionChanged;

	public DiagnosticCollection()
	{
		diagnostics.CollectionChanged += OnDiagnosticsOnCollectionChanged;
	}

	private void OnDiagnosticsOnCollectionChanged(object? _, NotifyCollectionChangedEventArgs e)
	{
		CollectionChanged?.Invoke(
			this,
			new DiagnosticCollectionChangedEventArgs(
				e.Action,
				e.NewItems?.Cast<Diagnostic>().ToImmutableList(),
				e.OldItems?.Cast<Diagnostic>().ToImmutableList()
			)
		);
	}

	public void Add(Diagnostic diagnostic) => diagnostics.Add(diagnostic);

	public void Add(DiagnosticArea area, Severity severity, string message, string code, Token? token = null,
		TokenLocation? location = null,
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
