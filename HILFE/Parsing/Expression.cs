using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing;

public class Expression
{
    public Expression(IReadOnlyList<Token> tokens)
    {
        Tokens = tokens;
    }

    public readonly IReadOnlyList<Token> Tokens;

    public ExpressionResult Evaluate(Interpreter interpreter)
    {
        if (Tokens.Count == 0)
            return new(null, true);

        if (Tokens.Count != 1)
            throw new NotImplementedException("Expressions with multiple tokens are not supported yet");

        var token = Tokens[0];
        if (token.Type == TokenType.Identifier)
        {
            return new(interpreter.CurrentScope.GetByIdentifier(token.Value).Value);
        }

        if (token.Type == TokenType.LiteralString)
        {
            return new(token.Value);
        }

        if (token.Type == TokenType.LiteralNumber)
        {
            return new (double.Parse(token.Value));
        }

        if (token.Type == TokenType.LiteralBoolean)
        {
            return new(bool.Parse(token.Value));
        }

        throw new NotImplementedException($"Expressions with {token.Type} tokens are not implemented");
    }
}