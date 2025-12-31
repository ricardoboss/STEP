using StepLang.Tooling.UI.Abstractions;
using StepLang.Tooling.UI.Raylib;

namespace StepLang.Tooling.UI;

public static class RenderingManager
{
	private static readonly IUiAdapter Adapter = new RaylibUiAdapter();

	private static IWindow? window { get; set; }

	public static bool IsWindowCreated => window is not null;

	public static IWindow Window => window ?? throw new InvalidOperationException("Create window first");

	public static async Task<IWindow> CreateWindow(WindowOptions options, CancellationToken cancellationToken = default)
	{
		return window = await Adapter.CreateWindow(options, cancellationToken);
	}

	public static async Task<IWindow> GetOrCreateWindow(WindowOptions options,
		CancellationToken cancellationToken = default)
	{
		if (IsWindowCreated)
			return window;

		return await CreateWindow(options, cancellationToken);
	}
}
