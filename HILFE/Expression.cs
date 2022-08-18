namespace HILFE;

public class Expression
{
    public Expression(IReadOnlyList<Token> tokens)
    {
        Tokens = tokens;
    }

    public readonly IReadOnlyList<Token> Tokens;

    public dynamic? Evaluate(Interpreter interpreter)
    {
        if (Tokens.Count == 0)
            throw new TokenizerException("Cannot evaluate empty expression!");

        // TODO: implement

        return null;
    }
}