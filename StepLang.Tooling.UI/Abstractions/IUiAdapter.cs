namespace StepLang.Tooling.UI.Abstractions;

public interface IUiAdapter
{
	Task<IWindow> CreateWindow(WindowOptions options, CancellationToken cancellationToken = default);
}

public class WindowOptions
{
	public required int Width { get; init; }

	public required int Height { get; init; }

	public required string Title { get; init; }
}
