using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record NullableVariableInitializationNode(TokenLocation AssignmentLocation, IEnumerable<Token> Types, Token NullabilityIndicator, Token Identifier, ExpressionNode Expression) : IVariableDeclarationNode
{
    public Variable EvaluateUsing(IVariableDeclarationEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }

    public TokenLocation Location => AssignmentLocation;

    public bool HasValue => true;
}