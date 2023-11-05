using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public interface IVariableDeclarationNode : IEvaluatableNode<IVariableDeclarationEvaluator, Variable>
{
    IReadOnlyCollection<Token> Types { get; }

    Token Identifier { get; }
}
