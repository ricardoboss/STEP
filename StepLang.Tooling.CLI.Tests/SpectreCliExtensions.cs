using Spectre.Console;
using Spectre.Console.Rendering;

namespace StepLang.Tooling.CLI.Tests;

public static class SpectreCliExtensions
{
	public static string GetTextContent(this Text text)
	{
		var readOnlyCapabilitiesMock = new Mock<IReadOnlyCapabilities>();

		var renderOptions =
			new RenderOptions(readOnlyCapabilitiesMock.Object, new Size(text.Length, 1));

		var segments = ((IRenderable)text).Render(renderOptions, text.Length);

		readOnlyCapabilitiesMock.VerifyAll();

		return string.Join("", segments.Select(s => s.Text));
	}
}
