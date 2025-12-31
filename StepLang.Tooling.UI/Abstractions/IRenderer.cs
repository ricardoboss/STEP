using System.Drawing;

namespace StepLang.Tooling.UI.Abstractions;

public interface IRenderer
{
	int MeasureText(string text);

	void DrawText(int x, int y, string text);

	void DrawLine(int x1, int y1, int x2, int y2);

	void Clear();

	Color Foreground { get; set; }

	Color Background { get; set; }

	int CanvasWidth { get; }

	int CanvasHeight { get; }

	int FontSize { get; }
}
