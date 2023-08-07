namespace StepLang.Framework.IO;

public class PrintlnFunction : PrintFunction
{
    public new const string Identifier = "println";

    /// <inheritdoc />
    protected override async Task Print(TextWriter output, string value, CancellationToken cancellationToken = default)
        => await output.WriteLineAsync(value.AsMemory(), cancellationToken);
}