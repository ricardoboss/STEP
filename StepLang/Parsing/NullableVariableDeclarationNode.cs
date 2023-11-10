using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record NullableVariableDeclarationNode(IReadOnlyCollection<Token> Types, Token NullabilityIndicator, Token Identifier) : IVariableDeclarationNode
{
    public Variable EvaluateUsing(IVariableDeclarationEvaluator evaluator)
    {
        return evaluator.Execute(this);
    }

    public TokenLocation Location => Types.First().Location;

    public bool HasValue => false;
}