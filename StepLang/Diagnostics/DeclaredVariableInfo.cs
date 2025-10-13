using StepLang.Parsing.Nodes.VariableDeclarations;

namespace StepLang.Diagnostics;

public sealed class DeclaredVariableInfo
{
	public required string Name { get; init; }

	public required Uri DocumentUri { get; init; }

	public required IVariableDeclarationNode Declaration { get; init; }

	public required VariableScope? DeclaringScope { get; init; }

	public override string ToString()
	{
		return $"DeclaredVariableInfo: \"{Name}\" in {Declaration.Location}";
	}
}
