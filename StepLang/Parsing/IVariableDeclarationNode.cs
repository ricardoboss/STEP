using StepLang.Tokenizing;

namespace StepLang.Parsing;

public interface IVariableDeclarationNode : IVisitableNode<IVariableDeclarationVisitor>
{
    IReadOnlyCollection<Token> Types { get; }

    Token Identifier { get; }
}
