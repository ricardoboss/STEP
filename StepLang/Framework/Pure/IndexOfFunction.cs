using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class IndexOfFunction : GenericFunction<ExpressionResult, ExpressionResult>
{
	public const string Identifier = "indexOf";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new([ResultType.List, ResultType.Map, ResultType.Str], "subject"),
		new(AnyValueType, "value"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } =
	[
		ResultType.Null, ResultType.Number, ResultType.Str,
	];

	protected override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		ExpressionResult argument1, ExpressionResult argument2)
	{
		return GetResult(argument1, argument2);
	}

	internal static ExpressionResult GetResult(ExpressionResult subject, ExpressionResult value)
	{
		return subject switch
		{
			ListResult list => GetListIndex(list, value),
			MapResult map => GetMapKey(map, value),
			StringResult haystack when value is StringResult needle => GetStringIndex(haystack, needle),
			_ => NullResult.Instance,
		};
	}

	private static NumberResult GetListIndex(ListResult list, ExpressionResult value)
	{
		return list.Value.IndexOf(value);
	}

	private static NumberResult GetStringIndex(StringResult haystack, StringResult needle)
	{
		return haystack.Value.GraphemeIndexOf(needle.Value);
	}

	private static ExpressionResult GetMapKey(MapResult map, ExpressionResult value)
	{
		var (key, _) = map.Value.FirstOrDefault(x => x.Value.Equals(value));

		return key == null ? NullResult.Instance : new StringResult(key);
	}
}
