namespace HILFE;

public class VariableDeclarationStatement : Statement
{
    /// <inheritdoc />
    public VariableDeclarationStatement(IReadOnlyList<Token> tokens) : base(StatementType.VariableDeclaration, tokens)
    {
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(Interpreter interpreter)
    {
        var typeToken = Tokens[0];
        if (typeToken.Type != TokenType.TypeName)
            throw new TokenizerException($"Expected {TokenType.TypeName} token, but got {typeToken.Type} instead");

        var type = typeToken.Value;

        var identifierToken = Tokens[1];
        if (identifierToken.Type != TokenType.Identifier)
            throw new TokenizerException($"Expected {TokenType.Identifier} token, but got {identifierToken.Type} instead");

        var identifier = identifierToken.Value;

        dynamic? value = null;
        if (Tokens.Count > 2)
        {
            var assignmentToken = Tokens[2];
            if (assignmentToken.Type != TokenType.EqualsSymbol)
                throw new TokenizerException($"Expected {TokenType.EqualsSymbol} token, but got {assignmentToken.Type} instead");

            var expression = new Expression(Tokens.Skip(3).ToList());
            value = expression.Evaluate(interpreter);
        }

        interpreter.Callstack.Peek().Variables.Add(identifier, new(type, value));
    }
}