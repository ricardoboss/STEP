using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class NonNullableVariableAssignmentException(
	TokenLocation location,
	Variable variable,
	ExpressionResult newValue)
	: InterpreterException(7, location,
		$"Cannot assign a value of type {newValue.ResultType.ToTypeName()} to a variable declared as {variable.TypeString}",
		$"Make sure the value you are trying to assign to {variable.Identifier} is of type {variable.TypeString} and not {ResultType.Null.ToTypeName()}.");
