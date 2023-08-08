using StepLang.Interpreting;
using StepLang.Parsing.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

public class InvalidVariableAssignmentException : IncompatibleTypesException
{
    public InvalidVariableAssignmentException(Token identifierToken, ExpressionResult result) : base(identifierToken.Location, $"Cannot assign value of type {result.ValueType} to variable {identifierToken.Value} of type {result.ValueType}", "Make sure the value you are assigning is of the same type as the variable you are assigning to.")
    {
    }
}