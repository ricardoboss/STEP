using StepLang.Tooling.UI.Abstractions;

namespace StepLang.Tooling.UI.Raylib;

public class RaylibUiAdapter : IUiAdapter
{
	public async Task<IWindow> CreateWindow(WindowOptions options, CancellationToken cancellationToken = default)
	{
		Raylib_cs.Raylib.InitWindow(options.Width, options.Height, options.Title);

		return new RaylibWindow();
	}
}
