using StepLang.Parsing.Statements;

namespace StepLang.Interpreting;

public class InvalidAssignmentException : InterpreterException
{
    public InvalidAssignmentException(Statement statement, Exception inner) : base(statement, $"Invalid assignment: {inner.Message}", inner)
    {
    }
}