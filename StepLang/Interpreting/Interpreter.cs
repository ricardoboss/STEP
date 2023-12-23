using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Interpreting;

/// <summary>
/// This class is responsible for interpreting a series of <see cref="StatementNode"/>s and handles the main execution loop.
/// </summary>
public partial class Interpreter : IRootNodeVisitor, IStatementVisitor, IExpressionEvaluator
{
    /// <summary>
    /// The standard output stream of this interpreter.
    /// </summary>
    public TextWriter? StdOut { get; }

    /// <summary>
    /// The standard error stream of this interpreter.
    /// </summary>
    public TextWriter? StdErr { get; }

    /// <summary>
    /// The standard input stream of this interpreter.
    /// </summary>
    public TextReader? StdIn { get; }

    /// <summary>
    /// The debug output stream of this interpreter.
    /// </summary>
    public TextWriter? DebugOut { get; }

    /// <summary>
    /// The exit code of this interpreter.
    /// </summary>
    public int ExitCode { get; set; }

    private readonly Stack<Scope> scopes = new();

    private Lazy<Random> random = new(() => new());

    /// <summary>
    /// Sets the random seed of this interpreter.
    /// </summary>
    /// <param name="value">The new random seed.</param>
    public void SetRandomSeed(int value) => random = new(() => new Random(value));

    /// <summary>
    /// The <see cref="Random"/> instance of this interpreter.
    /// </summary>
    public Random Random => random.Value;

    /// <summary>
    /// Creates a new <see cref="Interpreter"/> with the given streams.
    /// </summary>
    /// <param name="stdOut">The standard output stream of this interpreter.</param>
    /// <param name="stdErr">The standard error stream of this interpreter.</param>
    /// <param name="stdIn">The standard input stream of this interpreter.</param>
    /// <param name="debugOut">The debug output stream of this interpreter.</param>
    public Interpreter(TextWriter? stdOut = null, TextWriter? stdErr = null, TextReader? stdIn = null,
        TextWriter? debugOut = null)
    {
        StdOut = stdOut;
        StdErr = stdErr;
        StdIn = stdIn;
        DebugOut = debugOut;

        PushScope(Scope.GlobalScope);
    }

    /// <summary>
    /// The current <see cref="Scope"/> of this interpreter containing all defined identifiers and an optional result.
    /// </summary>
    public Scope CurrentScope => scopes.Peek();

    /// <summary>
    /// Pushes a new <see cref="Scope"/> to the stack of scopes.
    /// </summary>
    /// <param name="parent">The parent <see cref="Scope"/> of the new <see cref="Scope"/>.</param>
    /// <returns>The new <see cref="Scope"/>.</returns>
    public Scope PushScope(Scope? parent = null)
    {
        var newScope = new Scope(parent ?? CurrentScope);

        scopes.Push(newScope);

        DebugOut?.WriteLine($"Pushed new scope (new depth: {scopes.Count - 1})");

        return newScope;
    }

    /// <summary>
    /// Pops the current <see cref="Scope"/> from the stack of scopes.
    /// </summary>
    /// <returns>The popped <see cref="Scope"/>.</returns>
    public Scope PopScope()
    {
        DebugOut?.WriteLine($"Popping scope (new depth: {scopes.Count - 2})");

        return scopes.Pop();
    }

    /// <summary>
    /// <para>
    /// Interprets the given <see cref="StatementNode"/>s.
    /// </para>
    /// <para>
    /// This is the main execution loop of the interpreter.
    /// </para>
    /// </summary>
    /// <param name="statements">The <see cref="StatementNode"/>s to interpret.</param>
    public void Execute(IEnumerable<StatementNode> statements)
    {
        foreach (var statement in statements)
        {
            Execute(statement);

            if (CurrentScope.TryGetResult(out _, out _))
            {
                DebugOut?.WriteLine("Result found, continuing");

                return;
            }

            if (CurrentScope.ShouldBreak())
            {
                DebugOut?.WriteLine("Break found, breaking");

                return;
            }

            if (CurrentScope.ShouldContinue())
            {
                DebugOut?.WriteLine("Continue found, continuing");

                return;
            }
        }
    }

    public void Execute(StatementNode statement)
    {
        DebugOut?.WriteLine("Executing: " + statement);

        statement.Accept(this);
    }

    public void Run(RootNode node)
    {
        foreach (var importNode in node.Imports)
            importNode.Accept(this);

        Execute(node.Body);
    }

    public void Execute(CodeBlockStatementNode statementNode)
    {
        PushScope();

        Execute(statementNode.Body);

        if (PopScope().TryGetResult(out var resultValue, out var resultLocation))
            CurrentScope.SetResult(resultLocation, resultValue);
    }

    public ExpressionResult Evaluate(IdentifierExpressionNode expressionNode)
    {
        var variable = CurrentScope.GetVariable(expressionNode.Identifier);

        return variable.Value;
    }
}