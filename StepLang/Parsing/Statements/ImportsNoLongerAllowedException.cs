using System.Diagnostics.CodeAnalysis;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

public class ImportsNoLongerAllowedException : ParserException
{
    public ImportsNoLongerAllowedException(Token token) : base(token, "Import statement used after first statement", "Imports are only allowed before the first statement. You can use as many as you want, but as soon as any other statement is used, no more imports are allowed.")
    {
    }
}