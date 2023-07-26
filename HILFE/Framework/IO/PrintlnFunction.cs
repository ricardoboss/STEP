namespace HILFE.Framework.IO;

public class PrintlnFunction : PrintFunction
{
    /// <inheritdoc />
    protected override async Task Print(TextWriter output, string value, CancellationToken cancellationToken = default)
        => await output.WriteLineAsync(value.AsMemory(), cancellationToken);
}