namespace StepLang.Tooling.UI.Abstractions;

public interface IWindow
{
	IRenderer Renderer { get; }

	event EventHandler<FocusEventArgs> OnFocus;

	event EventHandler<BlurEventArgs> OnBlur;

	event EventHandler<PointerDownEventArgs> OnPointerDown;

	event EventHandler<PointerUpEventArgs> OnPointerUp;

	event EventHandler<PointerMoveEventArgs> OnPointerMove;

	event EventHandler<KeyDownEventArgs> OnKeyDown;

	event EventHandler<KeyUpEventArgs> OnKeyUp;
}

public class FocusEventArgs : EventArgs;

public class BlurEventArgs : EventArgs;

public class PointerDownEventArgs(int x, int y, int pointerIndex) : EventArgs;

public class PointerUpEventArgs(int x, int y, int pointerIndex) : EventArgs;

public class PointerMoveEventArgs(int dx, int dy, int pointerIndex) : EventArgs;

public class KeyDownEventArgs(int keyCode) : EventArgs;

public class KeyUpEventArgs(int keyCode) : EventArgs;
