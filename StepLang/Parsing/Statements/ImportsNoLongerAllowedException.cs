using System.Diagnostics.CodeAnalysis;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class ImportsNoLongerAllowedException : ParserException
{
    public ImportsNoLongerAllowedException(Token token) : base(token, "Imports are only allowed before the first statement")
    {
    }
}