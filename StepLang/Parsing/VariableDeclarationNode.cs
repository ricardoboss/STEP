using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record VariableDeclarationNode(IReadOnlyCollection<Token> Types, Token Identifier) : IVariableDeclarationNode
{
    public Variable EvaluateUsing(IVariableDeclarationEvaluator evaluator)
    {
        return evaluator.Execute(this);
    }

    public TokenLocation Location => Types.First().Location;
}