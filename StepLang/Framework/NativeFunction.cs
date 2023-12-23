using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework;

/// <summary>
/// The base class for all native functions.
/// </summary>
public abstract class NativeFunction : FunctionDefinition
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugBodyString => "[native code]";

    /// <summary>
    /// Represents a union type with all possible result types.
    /// </summary>
    protected static IReadOnlyList<ResultType> AnyType => Enum.GetValues<ResultType>();

    /// <summary>
    /// Represents a union type with all possible result types except <see cref="ResultType.Void"/>.
    /// </summary>
    protected static IReadOnlyList<ResultType> AnyValueType => Enum.GetValues<ResultType>().Except(new[] { ResultType.Void }).ToList();

    /// <summary>
    /// Represents a union type that only contains <see cref="ResultType.Number"/>.
    /// </summary>
    protected static IReadOnlyList<ResultType> OnlyNumber => new[] { ResultType.Number };

    /// <summary>
    /// Represents a union type that only contains <see cref="ResultType.Number"/> and <see cref="ResultType.Null"/>.
    /// </summary>
    protected static IReadOnlyList<ResultType> NullableNumber => new[] { ResultType.Null, ResultType.Number };

    /// <summary>
    /// Represents a union type that only contains <see cref="ResultType.Str"/>.
    /// </summary>
    protected static IReadOnlyList<ResultType> OnlyString => new[] { ResultType.Str };

    /// <summary>
    /// Represents a union type that only contains <see cref="ResultType.Str"/> and <see cref="ResultType.Null"/>.
    /// </summary>
    protected static IReadOnlyList<ResultType> NullableString => new[] { ResultType.Null, ResultType.Str };

    /// <summary>
    /// Represents a union type that only contains <see cref="ResultType.List"/>.
    /// </summary>
    protected static IReadOnlyList<ResultType> OnlyList => new[] { ResultType.List };

    /// <summary>
    /// Represents a union type that only contains <see cref="ResultType.Bool"/>.
    /// </summary>
    protected static IReadOnlyList<ResultType> OnlyBool => new[] { ResultType.Bool };

    /// <summary>
    /// Represents a union type that only contains <see cref="ResultType.Map"/>.
    /// </summary>
    protected static IReadOnlyList<ResultType> OnlyMap => new[] { ResultType.Map };

    /// <summary>
    /// Represents a union type that only contains <see cref="ResultType.Function"/>.
    /// </summary>
    protected static IReadOnlyList<ResultType> OnlyFunction => new[] { ResultType.Function };

    /// <summary>
    /// Checks if the number of arguments matches the number of parameters.
    /// </summary>
    /// <param name="location">The location of the function call.</param>
    /// <param name="arguments">The arguments passed to the function.</param>
    /// <exception cref="InvalidArgumentCountException">Thrown if the number of arguments does not match the number of parameters.</exception>
    protected void CheckArgumentCount(TokenLocation location, IReadOnlyList<ExpressionNode> arguments)
    {
        var expectedCount = Parameters.Count;
        if (arguments.Count != expectedCount)
            throw new InvalidArgumentCountException(location, expectedCount, arguments.Count);
    }

    /// <summary>
    /// Checks if the number of arguments is within the specified range.
    /// </summary>
    /// <param name="location">The location of the function call.</param>
    /// <param name="arguments">The arguments passed to the function.</param>
    /// <param name="minCount">The minimum number of arguments.</param>
    /// <param name="maxCount">The maximum number of arguments.</param>
    /// <exception cref="InvalidArgumentCountException">Thrown if the number of arguments is not within the specified range.</exception>
    protected static void CheckArgumentCount(TokenLocation location, IReadOnlyList<ExpressionNode> arguments, int minCount, int maxCount)
    {
        if (arguments.Count < minCount || arguments.Count > maxCount)
            throw new InvalidArgumentCountException(location, minCount, arguments.Count, maxCount);
    }

    /// <summary>
    /// Converts the <see cref="NativeParameters"/> to a list of <see cref="IVariableDeclarationNode"/>s.
    /// </summary>
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

    /// <summary>
    /// The native parameters of the function.
    /// </summary>
    protected abstract IEnumerable<NativeParameter> NativeParameters { get; }

    /// <summary>
    /// Represents a native parameter.
    /// </summary>
    /// <param name="Types">The types accepted by the parameter.</param>
    /// <param name="Identifier">The identifier of the parameter.</param>
    /// <param name="DefaultValue">The default value of the parameter.</param>
    protected record NativeParameter(IReadOnlyList<ResultType> Types, string Identifier, ExpressionNode? DefaultValue = null);
}