using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record VariableInitializationNode(IReadOnlyCollection<Token> Types, Token Identifier, ExpressionNode Expression) : IVariableDeclarationNode
{
    public Variable EvaluateUsing(IVariableDeclarationEvaluator evaluator)
    {
        return evaluator.Execute(this);
    }
}