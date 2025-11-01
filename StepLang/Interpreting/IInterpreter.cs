using StepLang.Parsing;
using StepLang.Parsing.Nodes.Statements;

namespace StepLang.Interpreting;

public interface IInterpreter : IRootNodeVisitor, IStatementVisitor, IExpressionEvaluator, IVariableDeclarationEvaluator
{
	TextWriter? StdOut { get; }

	TextReader? StdIn { get; }

	TextWriter? DebugOut { get; }

	double NextRandom();

	void SetRandomSeed(int value);

	Scope CurrentScope { get; }

	Scope PushScope(Scope? parent = null);

	Scope PopScope();

	TimeProvider Time { get; }

	int ExitCode { get; }
}
