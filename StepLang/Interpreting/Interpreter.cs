using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter : IRootNodeVisitor, IStatementVisitor, IExpressionEvaluator
{
    public TextWriter? StdOut { get; }
    public TextWriter? StdErr { get; }
    public TextReader? StdIn { get; }
    public TextWriter? DebugOut { get; }
    public int ExitCode { get; set; }

    public int BreakDepth { get; set; }

    public int ContinueDepth { get; set; }

    private readonly Stack<Scope> scopes = new();

    private Lazy<Random> random = new(() => new());

    public void SetRandomSeed(int value) => random = new(() => new Random(value));

    public Random Random => random.Value;

    public Interpreter(TextWriter? stdOut = null, TextWriter? stdErr = null, TextReader? stdIn = null,
        TextWriter? debugOut = null)
    {
        StdOut = stdOut;
        StdErr = stdErr;
        StdIn = stdIn;
        DebugOut = debugOut;

        PushScope(Scope.GlobalScope);
    }

    public Scope CurrentScope => scopes.Peek();

    public Scope PushScope(Scope? parent = null)
    {
        var newScope = new Scope(parent ?? CurrentScope);

        scopes.Push(newScope);

        DebugOut?.WriteLine($"Pushed new scope (new depth: {scopes.Count - 1})");

        return newScope;
    }

    public Scope PopScope()
    {
        DebugOut?.WriteLine($"Popping scope (new depth: {scopes.Count - 2})");

        return scopes.Pop();
    }

    public void Execute(IEnumerable<StatementNode> statements)
    {
        foreach (var statement in statements)
        {
            if (ContinueDepth > 0)
            {
                ContinueDepth--;

                DebugOut?.WriteLine("Continuing");

                break;
            }

            Execute(statement);

            if (!CurrentScope.TryGetResult(out _, out _))
                continue;

            DebugOut?.WriteLine("Result found, continuing");

            break;
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