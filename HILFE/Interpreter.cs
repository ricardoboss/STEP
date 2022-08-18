namespace HILFE;

public class Interpreter
{
    public readonly Config _config;
    public readonly Callstack Callstack;
    public readonly TextWriter StdOut;
    public readonly TextWriter StdErr;
    public readonly TextReader StdIn;

    public Interpreter(Config config, string entryFile, TextWriter stdout, TextWriter stderr, TextReader stdin)
    {
        _config = config;
        Callstack = new();
        Callstack.Push(new(null, entryFile, 0, 0));
        StdOut = stdout;
        StdErr = stderr;
        StdIn = stdin;
    }

    public async Task<int> InterpretAsync(IAsyncEnumerable<string> lines, CancellationToken cancellationToken = default)
    {
        var chars = lines.SelectMany(l => l.ToAsyncEnumerable());

        return await InterpretAsync(chars, cancellationToken);
    }

    public async Task<int> InterpretAsync(IAsyncEnumerable<char> input, CancellationToken cancellationToken = default)
    {
        var tokens = Tokenizer.TokenizeAsync(input, cancellationToken);
        var ast = Parser.ParseAsync(tokens, cancellationToken);

        await foreach (var statement in ast.WithCancellation(cancellationToken))
        {
            await StdOut.WriteLineAsync(statement.ToString());
            // await statement.ExecuteAsync(this);
        }

        return -1;
    }
}