using System.Diagnostics.CodeAnalysis;
using HILFE.Tokenizing;

namespace HILFE.Parsing;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class UnexpectedTokenException : ParserException
{
    public IReadOnlyCollection<TokenType> Allowed { get; }
    public Token Token { get; }

    public UnexpectedTokenException(Token token, params TokenType [] allowed) : base(
        $"Got '{token}' but expected one of '{string.Join(", ", allowed)}'")
    {
        Allowed = allowed;
        Token = token;
    }
}