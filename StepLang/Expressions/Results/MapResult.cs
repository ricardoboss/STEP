using System.Diagnostics.CodeAnalysis;

namespace StepLang.Expressions.Results;

public class MapResult : ValueExpressionResult<Dictionary<string, ExpressionResult>>
{
	public static MapResult Empty => new(new Dictionary<string, ExpressionResult>());

	/// <inheritdoc />
	public MapResult(Dictionary<string, ExpressionResult> value) : base(ResultType.Map, value)
	{
	}

	protected override bool EqualsInternal(ExpressionResult other)
	{
		return other is MapResult mapResult && Value.SequenceEqual(mapResult.Value);
	}

	public override MapResult DeepClone()
	{
		var clone = Value.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.DeepClone());

		return new MapResult(clone);
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return $"{{{string.Join(", ", Value.Select(p => $"{p.Key}: {p.Value}"))}}}";
	}

	public static implicit operator MapResult(Dictionary<string, ExpressionResult> value)
	{
		return new MapResult(value);
	}

	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "To prevent CA2225")]
	public static MapResult FromDictionary(Dictionary<string, ExpressionResult> value)
	{
		return value;
	}
}
