using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class IncompatibleVariableTypeException(TokenLocation location, Variable variable, ExpressionResult newValue)
	: InterpreterException(6, location,
		$"Cannot assign a value of type {newValue.ResultType.ToTypeName()} to a variable declared as {variable.TypeString}",
		$"Make sure the value you are trying to assign to {variable.Identifier} is of type {variable.TypeString}.");
