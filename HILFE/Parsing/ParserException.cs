namespace HILFE.Parsing;

public class ParserException : ApplicationException
{
    public readonly Parser.State? State;

    public ParserException(Parser.State state, string message) : base(message)
    {
        State = state;
    }
}