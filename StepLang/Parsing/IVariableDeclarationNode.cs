using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public interface IVariableDeclarationNode : IEvaluatableNode<IVariableDeclarationEvaluator, Variable>
{
    IReadOnlyCollection<Token> Types { get; }

    IEnumerable<ResultType> ResultTypes => Types
        .Where(t => t.Type == TokenType.TypeName)
        .Select(t => Expressions.Results.ResultTypes.FromTypeName(t.Value));

    Token Identifier { get; }
}
