using System.Drawing;
using StepLang.Tooling.UI.Abstractions;

namespace StepLang.Tooling.UI.Raylib;

public class RaylibRenderer : IRenderer
{
	public int MeasureText(string text)
	{
		throw new NotImplementedException();
	}

	public void DrawText(int x, int y, string text)
	{
		Raylib_cs.Raylib.BeginDrawing();
		Raylib_cs.Raylib.ClearBackground(RaylibBackground);
		Raylib_cs.Raylib.DrawText(text, x, y, FontSize, RaylibForeground);
		Raylib_cs.Raylib.EndDrawing();
	}

	public void DrawLine(int x1, int y1, int x2, int y2)
	{
		throw new NotImplementedException();
	}

	public void Clear()
	{
		throw new NotImplementedException();
	}

	public Color Foreground { get; set; } = Color.White;

	private Raylib_cs.Color RaylibForeground => new(Foreground.R, Foreground.G, Foreground.B);

	public Color Background { get; set; } = Color.Black;

	private Raylib_cs.Color RaylibBackground => new(Background.R, Background.G, Background.B);

	public int CanvasWidth { get; set; }

	public int CanvasHeight { get; set; }

	public int FontSize { get; set; }
}
