using System.Text.Json.Serialization;
using StepLang.Framework;
using StepLang.Framework.Conversion;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Expressions.Results;

[JsonConverter(typeof(ExpressionResultJsonConverter))]
public abstract class ExpressionResult : IEquatable<ExpressionResult>
{
    public static ExpressionResult DefaultFor(ResultType resultType)
    {
        return resultType switch
        {
            ResultType.Void => VoidResult.Instance,
            ResultType.Str => StringResult.Empty,
            ResultType.Number => NumberResult.Zero,
            ResultType.Bool => BoolResult.False,
            ResultType.List => ListResult.Empty,
            ResultType.Map => MapResult.Empty,
            ResultType.Function => FunctionResult.VoidFunction,
            ResultType.Null => NullResult.Instance,
            _ => throw new ArgumentOutOfRangeException(nameof(resultType), resultType, null),
        };
    }

    protected ExpressionResult(ResultType resultType) => ResultType = resultType;

    public ResultType ResultType { get; }

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as ExpressionResult);

    /// <inheritdoc />
    public bool Equals(ExpressionResult? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (ResultType != other.ResultType)
            return false;

        return EqualsInternal(other);
    }

    protected abstract bool EqualsInternal(ExpressionResult other);

    /// <inheritdoc />
    public override int GetHashCode() => (int)ResultType;

    /// <inheritdoc />
    public override string ToString() => ResultType.ToTypeName();

    public abstract ExpressionResult DeepClone();

    public bool IsTruthy()
    {
        return this switch
        {
            BoolResult { Value: true } => true,
            _ => false,
        };
    }

    public static implicit operator ExpressionNode(ExpressionResult result) => result.ToExpressionNode();

    public ExpressionNode ToExpressionNode()
    {
        return this switch
        {
            NullResult => new LiteralExpressionNode(new(TokenType.LiteralNull, "null")),
            BoolResult boolResult => new LiteralExpressionNode(new(TokenType.LiteralBoolean, boolResult ? "true" : "false")),
            NumberResult numberResult => new LiteralExpressionNode(new(TokenType.LiteralNumber, numberResult)),
            StringResult stringResult => new LiteralExpressionNode(new(TokenType.LiteralString, stringResult)),
            ListResult listResult => new ListExpressionNode(new(TokenType.OpeningSquareBracket, "["), listResult.Value.Select(result => result.ToExpressionNode()).ToList()),
            MapResult mapResult => new MapExpressionNode(new(TokenType.OpeningCurlyBracket, "{"), mapResult.Value.ToDictionary(kvp => new Token(TokenType.LiteralString, kvp.Key), kvp => kvp.Value.ToExpressionNode())),
            FunctionResult { Value: UserDefinedFunctionDefinition userDefinedFunctionDefinition } => new FunctionDefinitionExpressionNode(new(TokenType.OpeningParentheses, "("), userDefinedFunctionDefinition.Parameters, userDefinedFunctionDefinition.Body),
            FunctionResult { Value: NativeFunction nativeFunctionDefinition } => new NativeFunctionDefinitionExpressionNode(nativeFunctionDefinition),
            VoidResult => throw new InvalidOperationException("Cannot convert void result to expression node"),
            _ => throw new InvalidOperationException("Cannot convert unknown result type to expression node"),
        };
    }
}