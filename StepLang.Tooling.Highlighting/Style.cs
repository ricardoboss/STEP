using System.Drawing;

namespace StepLang.Tooling.Highlighting;

public record Style(Color Foreground, Color Background, bool Bold, bool Italic, bool Underline, bool Strikethrough);
