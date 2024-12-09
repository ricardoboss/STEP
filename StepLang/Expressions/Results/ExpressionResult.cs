using System.Text.Json.Serialization;
using StepLang.Framework;
using StepLang.Framework.Conversion;
using StepLang.Parsing;
using StepLang.Tokenizing;

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
	public override string ToString()
	{
		return ResultType.ToTypeName();
	}

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
		return this switch
		{
			NullResult => new LiteralExpressionNode(new Token(TokenType.LiteralNull, "null")),
			BoolResult boolResult => new LiteralExpressionNode(new Token(TokenType.LiteralBoolean,
				boolResult ? "true" : "false")),
			NumberResult numberResult => new LiteralExpressionNode(new Token(TokenType.LiteralNumber, numberResult)),
			StringResult stringResult => new LiteralExpressionNode(new Token(TokenType.LiteralString,
				stringResult.ToString())),
			ListResult listResult => new ListExpressionNode(new Token(TokenType.OpeningSquareBracket, "["),
				listResult.Value.Select(result => result.ToExpressionNode()).ToList()),
			MapResult mapResult => new MapExpressionNode(new Token(TokenType.OpeningCurlyBracket, "{"),
				mapResult.Value.ToDictionary(kvp => new Token(TokenType.LiteralString, $"\"{kvp.Key}\""),
					kvp => kvp.Value.ToExpressionNode())),
			FunctionResult { Value: UserDefinedFunctionDefinition userDefinedFunctionDefinition } => new
				FunctionDefinitionExpressionNode(new Token(TokenType.OpeningParentheses, "("),
					userDefinedFunctionDefinition.Parameters, userDefinedFunctionDefinition.Body),
			FunctionResult { Value: NativeFunction nativeFunctionDefinition } =>
				new NativeFunctionDefinitionExpressionNode(nativeFunctionDefinition),
			VoidResult => throw new InvalidOperationException("Cannot convert void result to expression node"),
			_ => throw new InvalidOperationException("Cannot convert unknown result type to expression node"),
		};
	}
}
