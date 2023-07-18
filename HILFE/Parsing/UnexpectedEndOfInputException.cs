namespace HILFE.Parsing;

public class UnexpectedEndOfInputException : ParserException
{
    /// <inheritdoc />
    public UnexpectedEndOfInputException(string message) : base(message)
    {
    }
}