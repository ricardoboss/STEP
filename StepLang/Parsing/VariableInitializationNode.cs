using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record VariableInitializationNode(TokenLocation AssignmentLocation, IReadOnlyCollection<Token> Types, Token Identifier, ExpressionNode Expression) : IVariableDeclarationNode
{
    public Variable EvaluateUsing(IVariableDeclarationEvaluator evaluator)
    {
        return evaluator.Execute(this);
    }

    public TokenLocation Location => AssignmentLocation;

    public bool HasValue => true;
}