using Spectre.Console;
using Spectre.Console.Rendering;
using StepLang.Diagnostics;
using System.Globalization;
using System.Text;

namespace StepLang.Tooling.CLI.Widgets;

public class DiagnosticLine(Diagnostic diagnostic) : IRenderable
{
	private readonly TextPath path = new(diagnostic.File?.FullName ?? "unknown") { LeafStyle = "bold" };
	private readonly Style severityStyle = diagnostic.Severity switch
	{
		Severity.Error => "bold red",
		Severity.Warning => "bold yellow",
		Severity.Suggestion => "aqua",
		Severity.Hint => "blue",
		_ => "default",
	};
	private readonly string severityLabel = diagnostic.Severity switch
	{
		Severity.Error => "error",
		Severity.Warning => "warning",
		Severity.Suggestion => "suggestion",
		Severity.Hint => "hint",
		_ => "",
	};

	public Measurement Measure(RenderOptions options, int maxWidth)
	{
		var pathMeasurement = path.Measure(options, maxWidth);

		var locationWidth = 1; // (
		locationWidth += diagnostic.Line.ToString(CultureInfo.InvariantCulture).Length;
		locationWidth++; // ,
		locationWidth += diagnostic.Column.ToString(CultureInfo.InvariantCulture).Length;
		locationWidth++; // )
		locationWidth++; // :
		locationWidth++; // space

		var severityWidth = severityLabel.Length;
		severityWidth++; // space
		severityWidth += diagnostic.Code.Length;
		severityWidth++; // :
		severityWidth++; // space

		var messageWidth = diagnostic.Message.Length;

		var totalWidth = pathMeasurement.Max + locationWidth + severityWidth + messageWidth;

		return new Measurement(totalWidth, totalWidth);
	}

	public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
	{
		foreach (var segment in path.Render(options, maxWidth))
			yield return segment;

		var sb = new StringBuilder();
		sb.Append('(');
		sb.Append(diagnostic.Line);
		sb.Append(',');
		sb.Append(diagnostic.Column);
		sb.Append("): ");
		yield return new Segment(sb.ToString());

		sb.Clear();
		sb.Append(severityLabel);
		sb.Append(' ');
		sb.Append(diagnostic.Code);
		yield return new Segment(sb.ToString(), severityStyle);

		sb.Clear();
		sb.Append(": ");
		sb.Append(diagnostic.Message);
		yield return new Segment(sb.ToString());

		yield return Segment.LineBreak;
	}
}
