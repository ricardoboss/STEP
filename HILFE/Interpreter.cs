namespace HILFE;

public class Interpreter
{
    private readonly Config _config;
    private readonly Callstack _callstack;
    private readonly TextReader _input;
    private readonly TextWriter _stdOut;
    private readonly TextWriter _stdErr;
    private readonly TextReader _stdIn;

    public Interpreter(Config config, string entryFile, TextReader input, TextWriter stdout, TextWriter stderr, TextReader stdin)
    {
        _config = config;
        _callstack = new();
        _callstack.Push(new(null, entryFile, 0, 0));
        _input = input;
        _stdOut = stdout;
        _stdErr = stderr;
        _stdIn = stdin;
    }

    public async Task<int> Interpret(CancellationToken cancellationToken = default)
    {
        while (await _input.ReadLineAsync() is { } line)
        {
            try
            {
                var tokens = Tokenizer.Tokenize(line)
                    .Where(t => t.Type != TokenType.Whitespace)
                    .ToList();

                if (!tokens.Any())
                    continue;

                await _stdOut.WriteLineAsync(string.Join(".", tokens));
            }
            catch (TokenizerException te)
            {
                await _stdErr.WriteLineAsync(nameof(TokenizerException) + ": " + te.Message);
            }
        }

        return -1;
    }
}