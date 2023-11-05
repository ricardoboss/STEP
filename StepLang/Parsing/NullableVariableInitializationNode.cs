using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record NullableVariableInitializationNode(IReadOnlyCollection<Token> Types, Token NullabilityIndicator, Token Identifier, ExpressionNode Expression) : IVariableDeclarationNode
{
    public Variable EvaluateUsing(IVariableDeclarationEvaluator evaluator)
    {
        return evaluator.Execute(this);
    }
}