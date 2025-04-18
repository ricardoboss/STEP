using StepLang.Expressions.Results;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

public class InvalidIndexOperatorException(
	string index,
	Token indexExpressionToken,
	ResultType resultType,
	string operation)
	: ParserException(4, indexExpressionToken,
		$"Invalid index expression: Cannot {operation} index {index} of a value of type {resultType.ToTypeName()}",
		"Make sure you're accessing an index of a value that supports indexing (like lists or maps).");
