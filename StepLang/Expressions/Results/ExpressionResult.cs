using StepLang.Framework;
using StepLang.Framework.Conversion;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace StepLang.Expressions.Results;

[JsonConverter(typeof(ExpressionResultJsonConverter))]
public abstract class ExpressionResult(ResultType resultType) : IEquatable<ExpressionResult>
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

	public ResultType ResultType { get; } = resultType;

	/// <inheritdoc />
	public override bool Equals(object? obj)
	{
		return Equals(obj as ExpressionResult);
	}

	/// <inheritdoc />
	public bool Equals(ExpressionResult? other)
	{
		if (ReferenceEquals(null, other))
		{
			return false;
		}

		if (ReferenceEquals(this, other))
		{
			return true;
		}

		if (ResultType != other.ResultType)
		{
			return false;
		}

		return EqualsInternal(other);
	}

	protected abstract bool EqualsInternal(ExpressionResult other);

	/// <inheritdoc />
	public override int GetHashCode()
	{
		return (int)ResultType;
	}

	/// <inheritdoc />
	[ExcludeFromCodeCoverage]
	public override string ToString() => ResultType.ToTypeName();

	public abstract ExpressionResult DeepClone();

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

	public static implicit operator ExpressionNode(ExpressionResult result)
	{
		return result.ToExpressionNode();
	}

	public ExpressionNode ToExpressionNode()
	{
		switch (this)
		{
			case NullResult:
				return LiteralExpressionNode.Null;
			case BoolResult boolResult:
				return LiteralExpressionNode.FromBoolean(boolResult.Value);
			case NumberResult numberResult:
				return LiteralExpressionNode.FromDouble(numberResult.Value);
			case StringResult stringResult:
				return LiteralExpressionNode.FromString(stringResult.Value);
			case ListResult listResult:
				return new ListExpressionNode(new Token(TokenType.OpeningSquareBracket, "["), listResult.Value.Select(result => result.ToExpressionNode()).ToList());
			case MapResult mapResult:
				return new MapExpressionNode(new Token(TokenType.OpeningCurlyBracket, "{"), mapResult.Value.ToDictionary(kvp => new Token(TokenType.LiteralString, $"\"{kvp.Key}\""), IExpressionNode (kvp) => kvp.Value.ToExpressionNode()));
			case FunctionResult functionResult:
				switch (functionResult.Value)
				{
					case UserDefinedFunctionDefinition userDefinedFunction:
						return new FunctionDefinitionExpressionNode(new Token(TokenType.OpeningParentheses, "("), userDefinedFunction.Parameters, userDefinedFunction.Body);
					case NativeFunction nativeFunction:
						return new NativeFunctionDefinitionExpressionNode(nativeFunction);
				}

				goto default;
			case VoidResult:
				throw new InvalidOperationException("Cannot convert void result to expression node");
			default:
				throw new InvalidOperationException("Cannot convert unknown result type to expression node");
		}
	}
}
