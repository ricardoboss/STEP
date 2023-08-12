namespace StepLang.Framework.IO;

public class PrintlnFunction : PrintFunction
{
    public override string Identifier { get; } = "println";

    /// <inheritdoc />
    protected override async Task Print(TextWriter output, string value, CancellationToken cancellationToken = default)
        => await output.WriteLineAsync(value.AsMemory(), cancellationToken);
}