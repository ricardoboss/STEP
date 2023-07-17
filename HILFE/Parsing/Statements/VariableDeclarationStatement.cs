using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class VariableDeclarationStatement : BaseStatement, IExecutableStatement
{
    /// <inheritdoc />
    public VariableDeclarationStatement(IReadOnlyList<Token> tokens) : base(StatementType.VariableDeclaration, tokens)
    {
    }

    /// <inheritdoc />
    public Task ExecuteAsync(Interpreter interpreter)
    {
        var meaningfulTokens = Tokens.OnlyMeaningful().ToArray();
        
        var typeToken = meaningfulTokens[0];
        if (typeToken.Type != TokenType.TypeName)
            throw new TokenizerException($"Expected {TokenType.TypeName} token, but got {typeToken.Type} instead");

        var type = typeToken.Value;

        var identifierToken = meaningfulTokens[1];
        if (identifierToken.Type != TokenType.Identifier)
            throw new TokenizerException($"Expected {TokenType.Identifier} token, but got {identifierToken.Type} instead");

        var identifier = identifierToken.Value;

        ExpressionResult? value = null;
        if (meaningfulTokens.Length > 2)
        {
            var assignmentToken = meaningfulTokens[2];
            if (assignmentToken.Type != TokenType.EqualsSymbol)
                throw new TokenizerException($"Expected {TokenType.EqualsSymbol} token, but got {assignmentToken.Type} instead");

            var expression = new Expression(meaningfulTokens.Skip(3).ToList());
            value = expression.Evaluate(interpreter);
        }

        if (value is not null or { IsVoid: true })
            interpreter.Scope.CurrentScope.AddIdentifier(identifier, new(identifier, type, value));

        return Task.CompletedTask;
    }
}