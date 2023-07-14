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

        if (Tokens.Count != 1)
            throw new NotImplementedException("Expressions with multiple tokens are not supported yet");

        var token = Tokens[0];
        if (token.Type == TokenType.Identifier)
        {
            return interpreter.Scope.CurrentScope.GetByIdentifier(token.Value).Value;
        }

        if (token.Type == TokenType.LiteralString)
        {
            return token.Value;
        }

        if (token.Type == TokenType.LiteralNumber)
        {
            return double.Parse(token.Value);
        }

        throw new NotImplementedException($"Expressions with {token.Type} tokens are not implemented");
    }
}