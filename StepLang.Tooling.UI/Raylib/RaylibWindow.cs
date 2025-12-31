using StepLang.Tooling.UI.Abstractions;

namespace StepLang.Tooling.UI.Raylib;

public class RaylibWindow : IWindow
{
	public IRenderer Renderer { get; } = new RaylibRenderer();

	public event EventHandler<FocusEventArgs>? OnFocus;

	public event EventHandler<BlurEventArgs>? OnBlur;

	public event EventHandler<PointerDownEventArgs>? OnPointerDown;

	public event EventHandler<PointerUpEventArgs>? OnPointerUp;

	public event EventHandler<PointerMoveEventArgs>? OnPointerMove;

	public event EventHandler<KeyDownEventArgs>? OnKeyDown;

	public event EventHandler<KeyUpEventArgs>? OnKeyUp;
}
