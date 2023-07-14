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

        if (Tokens.Count == 1 && Tokens[0].Type == TokenType.Identifier)
        {
            return interpreter.Scope.CurrentScope.GetByIdentifier(Tokens[0].Value).Value;
        }

        return null;
    }
}