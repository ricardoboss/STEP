using StepLang.Diagnostics;

namespace StepLang.Tooling.Diagnostics;

public sealed class FileSymbols
{
	public required Uri DocumentUri { get; init; }

	public required IReadOnlyList<DeclaredVariableInfo> Declarations { get; init; }

	public required IReadOnlyList<VariableScope> Scopes { get; init; }
}
