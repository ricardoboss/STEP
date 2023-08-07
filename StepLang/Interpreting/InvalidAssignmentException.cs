using System.Diagnostics.CodeAnalysis;
using StepLang.Parsing.Statements;

namespace StepLang.Interpreting;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class InvalidAssignmentException : InterpreterException
{
    public InvalidAssignmentException(Statement statement, Exception inner) : base($"{statement.Location?.ToString() ?? "<unknown>"}: Invalid assignment: {inner.Message}", inner)
    {
    }
}