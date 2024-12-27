using Spectre.Console.Rendering;

namespace StepLang.Tooling.CLI.Widgets;

public sealed class DefinitionListItem(IRenderable label, IRenderable definition)
{
	public IRenderable Label { get; } = label;

	public IRenderable Definition { get; } = definition;
}
