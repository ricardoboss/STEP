using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes;
using StepLang.Tokenizing;
using System.Text.RegularExpressions;

namespace StepLang.Framework.Other;

public class RegexMatchFunction : GenericFunction<StringResult, StringResult, ExpressionResult>
{
	public const string Identifier = "regexMatch";

	protected override IEnumerable<NativeParameter> NativeParameters =>
	[
		new(OnlyString, "regex"),
		new(OnlyString, "input"),
		new(NullableString, "options", LiteralExpressionNode.Null),
	];

	protected override IEnumerable<ResultType> ReturnTypes => OnlyMap;

	protected override ListResult Invoke(TokenLocation callLocation, Interpreter interpreter, StringResult argument1,
		StringResult argument2, ExpressionResult argument3)
	{
		var options = RegexOptions.None;
		if (argument3 is StringResult { Value: { } optionsString })
		{
			options = optionsString.Aggregate(options, (current, c) => current | c switch
			{
				'i' => RegexOptions.IgnoreCase,
				'm' => RegexOptions.Multiline,
				'n' => RegexOptions.ExplicitCapture,
				's' => RegexOptions.Singleline,
				'x' => RegexOptions.IgnorePatternWhitespace,
				'R' => RegexOptions.RightToLeft,
				'e' => RegexOptions.ECMAScript,
				'c' => RegexOptions.CultureInvariant,
				'N' => RegexOptions.NonBacktracking,
				_ => throw new InvalidArgumentValueException(callLocation, $"Invalid RegEx options: {argument3}"),
			});
		}

		var regex = new Regex(argument1.Value, options);
		var matches = regex.Matches(argument2.Value);

		return matches.Select(m =>
		{
			var matchResult = ConvertToMapResult(m);

			matchResult.Value["groups"] = m.Groups.AsReadOnly().ToDictionary(
				g => g.Name,
				g =>
				{
					var groupResult = ConvertToMapResult(g);

					groupResult.Value["captures"] = g.Captures.Select(ConvertToMapResult).ToListResult();

					return groupResult;
				}).ToMapResult();

			return matchResult;
		}).ToListResult();
	}

	private static MapResult ConvertToMapResult(Capture capture)
	{
		return new Dictionary<string, ExpressionResult>
		{
			["value"] = capture.Value.ToStringResult(),
			["index"] = capture.Index.ToNumberResult(),
			["length"] = capture.Length.ToNumberResult(),
		};
	}
}
