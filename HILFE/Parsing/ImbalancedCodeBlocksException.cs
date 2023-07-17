namespace HILFE.Parsing;

public class ImbalancedCodeBlocksException : UnexpectedEndOfInputException
{
    /// <inheritdoc />
    public ImbalancedCodeBlocksException(Parser.State state, string message) : base(state, message)
    {
    }
}