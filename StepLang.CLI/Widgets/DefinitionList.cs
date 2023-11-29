using Spectre.Console.Rendering;

namespace StepLang.CLI.Widgets;

internal sealed class DefinitionList : IRenderable
{
    public sealed class Item
    {
        public Item(IRenderable label, IRenderable definition)
        {
            Label = label;
            Definition = definition;
        }

        public IRenderable Label { get; }

        public IRenderable Definition { get; }
    }

    public List<Item> Items { get; init; } = [];

    public int ItemIndent { get; init; } = 4;

    public bool Expand { get; init; }

    public Measurement Measure(RenderOptions options, int maxWidth)
    {
        if (Expand)
            return new(maxWidth, maxWidth);

        var maxLabelWidth = Items.Select(i => i.Label.Measure(options, maxWidth).Max).Max();
        var maxDefinitionWidth = Items.Select(i => i.Definition.Measure(options, maxWidth - ItemIndent).Max).Max() + ItemIndent;

        var max = Math.Max(maxLabelWidth, maxDefinitionWidth);

        return new(max, max);
    }

    public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        foreach (var item in Items)
        {
            foreach (var labelSegment in item.Label.Render(options, maxWidth))
                yield return labelSegment;

            yield return Segment.LineBreak;
            yield return Segment.Padding(ItemIndent);

            foreach (var definitionSegment in item.Definition.Render(options, maxWidth - ItemIndent))
                yield return definitionSegment;

            yield return Segment.LineBreak;
        }
        yield return Segment.LineBreak;
    }
}