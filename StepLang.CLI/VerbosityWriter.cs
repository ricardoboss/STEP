namespace StepLang.CLI;

internal sealed record VerbosityWriter(TextWriter Writer, Verbosity Verbosity)
{
    public async Task Quiet(string value)
    {
        if (Verbosity.IsAtLeast(Verbosity.Quiet))
            await Writer.WriteLineAsync(value);
    }

    public async Task Normal(string value)
    {
        if (Verbosity.IsAtLeast(Verbosity.Normal))
            await Writer.WriteLineAsync(value);
    }

    public async Task Verbose(string value)
    {
        if (Verbosity.IsAtLeast(Verbosity.Verbose))
            await Writer.WriteLineAsync(value);
    }
}