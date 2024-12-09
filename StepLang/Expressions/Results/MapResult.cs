using System.Diagnostics.CodeAnalysis;
using Map = System.Collections.Generic.Dictionary<string, StepLang.Expressions.Results.ExpressionResult>;

namespace StepLang.Expressions.Results;

public class MapResult : ValueExpressionResult<Map>
{
	public static MapResult Empty => new(new Map());

	/// <inheritdoc />
	public MapResult(Map value) : base(ResultType.Map, value)
	{
	}

	protected override bool EqualsInternal(ExpressionResult other)
	{
		return other is MapResult mapResult && Value.SequenceEqual(mapResult.Value, MapEqualityComparer.Default);
	}

	private class MapEqualityComparer : EqualityComparer<KeyValuePair<string, ExpressionResult>>
	{
		public static new readonly MapEqualityComparer Default = new();

		public override bool Equals(KeyValuePair<string, ExpressionResult> x, KeyValuePair<string, ExpressionResult> y)
		{
			return x.Key == y.Key && x.Value.Equals(y.Value);
		}

		public override int GetHashCode(KeyValuePair<string, ExpressionResult> obj)
		{
			return HashCode.Combine(obj.Key, obj.Value);
		}
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

	public static implicit operator MapResult(Map value)
	{
		return new MapResult(value);
	}

	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "To prevent CA2225")]
	public static MapResult FromDictionary(Map value)
	{
		return value;
	}
}
