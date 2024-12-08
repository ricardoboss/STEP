using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public abstract class BaseVariable
{
    public abstract void Assign(TokenLocation assignmentLocation, ExpressionResult newValue);

    public abstract ExpressionResult Value { get; }
}
