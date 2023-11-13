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

    protected static IReadOnlyList<ResultType> AnyType => Enum.GetValues<ResultType>();
    protected static IReadOnlyList<ResultType> AnyValueType => Enum.GetValues<ResultType>().Except(new[] { ResultType.Void }).ToList();
    protected static IReadOnlyList<ResultType> OnlyNumber => new[] { ResultType.Number };
    protected static IReadOnlyList<ResultType> NullableNumber => new[] { ResultType.Null, ResultType.Number };
    protected static IReadOnlyList<ResultType> OnlyString => new[] { ResultType.Str };
    protected static IReadOnlyList<ResultType> NullableString => new[] { ResultType.Null, ResultType.Str };
    protected static IReadOnlyList<ResultType> OnlyList => new[] { ResultType.List };
    protected static IReadOnlyList<ResultType> OnlyBool => new[] { ResultType.Bool };
    protected static IReadOnlyList<ResultType> OnlyMap => new[] { ResultType.Map };
    protected static IReadOnlyList<ResultType> OnlyFunction => new[] { ResultType.Function };

    protected void CheckArgumentCount(TokenLocation location, IReadOnlyList<ExpressionNode> arguments)
    {
        var expectedCount = Parameters.Count;
        if (arguments.Count != expectedCount)
            throw new InvalidArgumentCountException(location, expectedCount, arguments.Count);
    }

    protected static void CheckArgumentCount(TokenLocation location, IReadOnlyList<ExpressionNode> arguments, int minCount, int maxCount)
    {
        if (arguments.Count < minCount || arguments.Count > maxCount)
            throw new InvalidArgumentCountException(location, minCount, arguments.Count, maxCount);
    }

    public override IReadOnlyCollection<IVariableDeclarationNode> Parameters => NativeParameters
        .Select<NativeParameter, IVariableDeclarationNode>(p =>
        {
            var types = p.Types.Select(t => new Token(TokenType.TypeName, t.ToTypeName())).ToList();
            var nullable = p.Types.Any(t => t is ResultType.Null);
            var identifier = new Token(TokenType.Identifier, p.Identifier);

            if (p.DefaultValue is null)
            {
                if (!nullable)
                    return new VariableDeclarationNode(types, identifier);

                return new NullableVariableDeclarationNode(types, new(TokenType.QuestionMarkSymbol, "?"), identifier);

            }

            if (!nullable)
                return new VariableInitializationNode(new(), types, identifier, p.DefaultValue);

            return new NullableVariableInitializationNode(new(), types, new(TokenType.QuestionMarkSymbol, "?"), identifier, p.DefaultValue);
        })
        .ToList();

    protected abstract IEnumerable<NativeParameter> NativeParameters { get; }

    protected record NativeParameter(IReadOnlyList<ResultType> Types, string Identifier, ExpressionNode? DefaultValue = null);
}