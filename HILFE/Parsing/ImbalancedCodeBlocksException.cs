namespace HILFE.Parsing;

public class ImbalancedCodeBlocksException : UnexpectedEndOfInputException
{
    /// <inheritdoc />
    public ImbalancedCodeBlocksException(string message) : base(message)
    {
    }
}