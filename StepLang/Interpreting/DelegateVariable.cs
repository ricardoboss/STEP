using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public delegate ExpressionResult VariableEvaluator();

public delegate void VariableAssigner(TokenLocation assignmentLocation, ExpressionResult newValue);

public class DelegateVariable : BaseVariable
{
	public required VariableEvaluator Evaluator { get; init; }
	public required VariableAssigner Assigner { get; init; }

	public override void Assign(TokenLocation assignmentLocation, ExpressionResult newValue)
	{
		Assigner(assignmentLocation, newValue);
	}

	public override ExpressionResult Value => Evaluator();
}
