using Spectre.Console.Rendering;

namespace StepLang.Tooling.CLI.Widgets;

public sealed class DefinitionList : IRenderable
{
	public IList<DefinitionListItem> Items { get; init; } = [];

	public int ItemIndent { get; init; } = 4;

	public bool Expand { get; init; }

	public Measurement Measure(RenderOptions options, int maxWidth)
	{
		if (Expand)
		{
			return new Measurement(maxWidth, maxWidth);
		}

		var maxLabelWidth = Items.Select(i => i.Label.Measure(options, maxWidth).Max).Max();
		var maxDefinitionWidth = Items.Select(i => i.Definition.Measure(options, maxWidth - ItemIndent).Max).Max() +
								 ItemIndent;

		var max = Math.Max(maxLabelWidth, maxDefinitionWidth);

		return new Measurement(max, max);
	}

	public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
	{
		foreach (var item in Items)
		{
			foreach (var labelSegment in item.Label.Render(options, maxWidth))
			{
				yield return labelSegment;
			}

			yield return Segment.LineBreak;
			yield return Segment.Padding(ItemIndent);

			foreach (var definitionSegment in item.Definition.Render(options, maxWidth - ItemIndent))
			{
				yield return definitionSegment;

				if (definitionSegment.IsLineBreak)
					yield return Segment.Padding(ItemIndent);
			}

			yield return Segment.LineBreak;
		}

		yield return Segment.LineBreak;
	}
}
