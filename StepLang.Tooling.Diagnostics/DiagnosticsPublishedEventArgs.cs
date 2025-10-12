using StepLang.Diagnostics;
using System.Collections.ObjectModel;

namespace StepLang.Tooling.Diagnostics;

public sealed class DiagnosticsPublishedEventArgs : EventArgs
{
	public required Uri DocumentUri { get; init; }

	public required ObservableCollection<Diagnostic> Diagnostics { get; init; }

	public int Version { get; set; }
}
