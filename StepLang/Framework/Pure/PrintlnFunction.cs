namespace StepLang.Framework.Pure;

/// <summary>
/// Prints to STDOUT with a newline.
/// </summary>
public class PrintlnFunction : PrintFunction
{
    /// <summary>
    /// The identifier of the <see cref="PrintlnFunction"/> function.
    /// </summary>
    public new const string Identifier = "println";

    /// <inheritdoc />
    protected override void Print(TextWriter output, string value)
        => output.WriteLine(value.AsMemory());
}