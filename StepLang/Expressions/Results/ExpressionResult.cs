using System.Text.Json.Serialization;
using StepLang.Framework;
using StepLang.Framework.Conversion;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Expressions.Results;

/// <summary>
/// <para>
/// This is the base class for all <see cref="ExpressionResult"/> results.
/// </para>
/// <para>
/// An expression result is the result of evaluating an expression using an <see cref="StepLang.Interpreting.Interpreter"/>.
/// Some examples include <see cref="NumberResult"/>, <see cref="StringResult"/> or <see cref="FunctionResult"/>.
/// The result of an expression can be saved in a <see cref="StepLang.Interpreting.Variable"/>
/// </para>
/// </summary>
[JsonConverter(typeof(ExpressionResultJsonConverter))]
public abstract class ExpressionResult : IEquatable<ExpressionResult>
{
    /// <summary>
    /// Returns the default value for the given <see cref="ResultType"/>.
    /// </summary>
    /// <param name="resultType">The <see cref="ResultType"/> to get the default value for.</param>
    /// <returns>
    /// <para>
    /// The default value for the given <see cref="ResultType"/>.
    /// </para>
    /// <list type="bullet">
    /// <listheader><description>The default values are:</description></listheader>
    /// <item><description><see cref="VoidResult"/>.<see cref="VoidResult.Instance"/> for <see cref="ResultType.Void"/></description></item>
    /// <item><description><see cref="StringResult"/>.<see cref="StringResult.Empty"/> for <see cref="ResultType.Str"/></description></item>
    /// <item><description><see cref="NumberResult"/>.<see cref="NumberResult.Zero"/> for <see cref="ResultType.Number"/></description></item>
    /// <item><description><see cref="BoolResult"/>.<see cref="BoolResult.False"/> for <see cref="ResultType.Bool"/></description></item>
    /// <item><description><see cref="ListResult"/>.<see cref="ListResult.Empty"/> for <see cref="ResultType.List"/></description></item>
    /// <item><description><see cref="MapResult"/>.<see cref="MapResult.Empty"/> for <see cref="ResultType.Map"/></description></item>
    /// <item><description><see cref="FunctionResult"/>.<see cref="FunctionResult.VoidFunction"/> for <see cref="ResultType.Function"/></description></item>
    /// <item><description><see cref="NullResult"/>.<see cref="NullResult.Instance"/> for <see cref="ResultType.Null"/></description></item>
    /// </list>
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="resultType"/> is not a valid <see cref="ResultType"/>.</exception>
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

    /// <summary>
    /// Creates a new <see cref="ExpressionResult"/> with the given <see cref="ResultType"/>.
    /// </summary>
    /// <param name="resultType">The <see cref="ResultType"/> of the new <see cref="ExpressionResult"/>.</param>
    protected ExpressionResult(ResultType resultType) => ResultType = resultType;

    /// <summary>
    /// The <see cref="ResultType"/> of this <see cref="ExpressionResult"/>.
    /// </summary>
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

    /// <summary>
    /// Checks if this <see cref="ExpressionResult"/> is equal to <paramref name="other"/>.
    /// </summary>
    /// <param name="other">The <see cref="ExpressionResult"/> to compare to.</param>
    /// <returns><c>true</c> if this <see cref="ExpressionResult"/> is equal to <paramref name="other"/>, <c>false</c> otherwise.</returns>
    protected abstract bool EqualsInternal(ExpressionResult other);

    /// <inheritdoc />
    public override int GetHashCode() => (int)ResultType;

    /// <inheritdoc />
    public override string ToString() => ResultType.ToTypeName();

    /// <summary>
    /// Creates a new <see cref="ExpressionResult"/> with the same <see cref="ResultType"/> (and value for <see cref="ValueExpressionResult{T}"/>s) as this result.
    /// </summary>
    /// <returns>A new <see cref="ExpressionResult"/> with the same <see cref="ResultType"/> (and value for <see cref="ValueExpressionResult{T}"/>s) as this result.</returns>
    public abstract ExpressionResult DeepClone();

    /// <summary>
    /// <para>
    /// Determines whether the given <see cref="ExpressionResult"/> is considered <c>true</c> when used in a boolean context.
    /// </para>
    /// <para>
    /// A value is considered <c>true</c> when:
    /// <list type="bullet">
    /// <item><description>It is a <see cref="BoolResult"/> with a value of <c>true</c>.</description></item>
    /// <item><description>It is a <see cref="StringResult"/> with a value of <c>"1"</c>.</description></item>
    /// <item><description>It is a <see cref="StringResult"/> that can be parsed to a <see cref="bool"/>.</description></item>
    /// <item><description>It is a <see cref="NumberResult"/> with a value greater than <c>0</c>.</description></item>
    /// </list>
    /// All other values are considered <c>false</c>.
    /// </para>
    /// </summary>
    /// <returns><c>true</c> if the given <see cref="ExpressionResult"/> is considered <c>true</c> when used in a boolean context, <c>false</c> otherwise.</returns>
    public bool IsTruthy()
    {
        return this switch
        {
            BoolResult { Value: true } => true,
            StringResult { Value: "1" } => true,
            StringResult { Value: var strValue } when bool.TryParse(strValue, out var value) => value,
            NumberResult { Value: > 0 } => true,
            _ => false,
        };
    }

    /// <summary>
    /// Converts this <see cref="ExpressionResult"/> to an <see cref="ExpressionNode"/>.
    /// </summary>
    /// <returns>An <see cref="ExpressionNode"/> representing this <see cref="ExpressionResult"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if this <see cref="ExpressionResult"/> cannot be converted to an <see cref="ExpressionNode"/>.</exception>
    public ExpressionNode ToExpressionNode()
    {
        return this switch
        {
            NullResult => new LiteralExpressionNode(new(TokenType.LiteralNull, "null")),
            BoolResult boolResult => new LiteralExpressionNode(new(TokenType.LiteralBoolean, boolResult ? "true" : "false")),
            NumberResult numberResult => new LiteralExpressionNode(new(TokenType.LiteralNumber, numberResult)),
            StringResult stringResult => new LiteralExpressionNode(new(TokenType.LiteralString, stringResult.ToString())),
            ListResult listResult => new ListExpressionNode(new(TokenType.OpeningSquareBracket, "["), listResult.Value.Select(result => result.ToExpressionNode()).ToList()),
            MapResult mapResult => new MapExpressionNode(new(TokenType.OpeningCurlyBracket, "{"), mapResult.Value.ToDictionary(kvp => new Token(TokenType.LiteralString, $"\"{kvp.Key}\""), kvp => kvp.Value.ToExpressionNode())),
            FunctionResult { Value: UserDefinedFunctionDefinition userDefinedFunctionDefinition } => new FunctionDefinitionExpressionNode(new(TokenType.OpeningParentheses, "("), userDefinedFunctionDefinition.Parameters, userDefinedFunctionDefinition.Body),
            FunctionResult { Value: NativeFunction nativeFunctionDefinition } => new NativeFunctionDefinitionExpressionNode(nativeFunctionDefinition),
            VoidResult => throw new InvalidOperationException("Cannot convert void result to expression node"),
            _ => throw new InvalidOperationException("Cannot convert unknown result type to expression node"),
        };
    }
}