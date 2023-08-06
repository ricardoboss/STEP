using System.Diagnostics.CodeAnalysis;
using STEP.Tokenizing;

namespace STEP.Parsing;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class UnexpectedTokenException : ParserException
{
    public UnexpectedTokenException(Token token, params TokenType [] allowed) : base(
        $"Got '{token}' but expected one of '{string.Join(", ", allowed)}'")
    {
    }
}