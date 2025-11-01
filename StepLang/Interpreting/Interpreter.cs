using StepLang.Diagnostics;
using StepLang.Expressions.Results;
using StepLang.Parsing.Nodes;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Parsing.Nodes.Statements;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Interpreting;

public partial class Interpreter : IInterpreter
{
	public TextWriter? StdOut { get; }
	public TextReader? StdIn { get; }
	public TextWriter? DebugOut { get; }

	public DiagnosticCollection Diagnostics { get; }

	public int ExitCode { get; set; }

	private readonly Stack<Scope> scopes = new();

	private Lazy<Random> random = new(() => new Random());

	public void SetRandomSeed(int value)
	{
		random = new Lazy<Random>(() => new Random(value));
	}

	[SuppressMessage("Security", "CA5394:Do not use insecure randomness",
		Justification = "This is not for security purposes")]
	public double NextRandom() => random.Value.NextDouble();

	public TimeProvider Time { get; } = TimeProvider.System;

	public Interpreter(TextWriter? stdOut = null, TextReader? stdIn = null, TextWriter? debugOut = null,
		DiagnosticCollection? diagnostics = null)
	{
		StdOut = stdOut;
		StdIn = stdIn;
		DebugOut = debugOut;
		Diagnostics = diagnostics ?? [];

		_ = PushScope(Scope.GlobalScope);
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

	private void Execute(IEnumerable<StatementNode> statements)
	{
		foreach (var statement in statements)
		{
			statement.Accept(this);

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

	public void Visit(RootNode node)
	{
		foreach (var importNode in node.Imports)
		{
			importNode.Accept(this);
		}

		Execute(node.Body);
	}

	public void Visit(CodeBlockStatementNode statementNode)
	{
		_ = PushScope();

		Execute(statementNode.Statements);

		var resultScope = PopScope();

		if (resultScope.TryGetResult(out var resultValue, out var resultLocation))
		{
			CurrentScope.SetResult(resultLocation, resultValue);
		}
		else if (resultScope.ShouldBreak())
		{
			CurrentScope.SetBreak();
		}
		else if (resultScope.ShouldContinue())
		{
			CurrentScope.SetContinue();
		}
	}

	public void Visit(ErrorStatementNode errorStatementNode)
	{
		throw new NotSupportedException("Error statement nodes cannot be interpreted");
	}

	public ExpressionResult Evaluate(IdentifierExpressionNode expressionNode)
	{
		var variable = CurrentScope.GetVariable(expressionNode.Identifier);

		return variable.Value;
	}

	public ExpressionResult Evaluate(ErrorExpressionNode expressionNode)
	{
		throw new NotSupportedException("Error expression nodes cannot be interpreted");
	}
}
