namespace HILFE.Parsing;

public class UnexpectedEndOfInputException : ParserException
{
    /// <inheritdoc />
    public UnexpectedEndOfInputException(Parser.State state, string message) : base(state, message)
    {
    }
}