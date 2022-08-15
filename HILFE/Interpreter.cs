namespace HILFE;

public class Interpreter
{
    private readonly Config _config;
    private readonly Callstack _callstack;
    private readonly TextWriter _stdOut;
    private readonly TextWriter _stdErr;
    private readonly TextReader _stdIn;

    public Interpreter(Config config, string entryFile, TextWriter stdout, TextWriter stderr, TextReader stdin)
    {
        _config = config;
        _callstack = new();
        _callstack.Push(new(null, entryFile, 0, 0));
        _stdOut = stdout;
        _stdErr = stderr;
        _stdIn = stdin;
    }

    public void Interpret(string line)
    {
        _stdOut.WriteLine("Interpreted line: " + line);
    }
}