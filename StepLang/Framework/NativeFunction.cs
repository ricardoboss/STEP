using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework;

public abstract class NativeFunction : FunctionDefinition
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugBodyString => "[native code]";

    protected void CheckArgumentCount(IReadOnlyList<ExpressionNode> arguments)
    {
        var expectedCount = Parameters.Count;
        if (arguments.Count != expectedCount)
            throw new InvalidArgumentCountException(expectedCount, arguments.Count);
    }

    protected static void CheckArgumentCount(IReadOnlyList<ExpressionNode> arguments, int minCount, int maxCount)
    {
        if (arguments.Count < minCount || arguments.Count > maxCount)
            throw new InvalidArgumentCountException(minCount, arguments.Count, maxCount);
    }

    public override IReadOnlyCollection<IVariableDeclarationNode> Parameters => NativeParameters.Select(p =>
    {
        var (types, identifier) = p;
        return new VariableDeclarationNode(types.Select(t => new Token(TokenType.TypeName, t.ToTypeName())).ToList(), new(TokenType.Identifier, identifier));
    }).ToList();

    protected virtual IEnumerable<(ResultType[] types, string identifier)> NativeParameters => Enumerable.Empty<(ResultType[], string)>();
}