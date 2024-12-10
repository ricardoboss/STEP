using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;
using System.Text;

namespace StepLang.Framework.Other;

public class FileReadFunction : GenericFunction<StringResult>
{
	public const string Identifier = "fileRead";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyString, "path"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = NullableString;

	protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
		StringResult argument1)
	{
		var path = argument1.Value;
		var info = callLocation.GetFileInfoFromPath(path);

		if (!info.Exists)
		{
			return NullResult.Instance;
		}

		try
		{
			var contents = File.ReadAllText(info.FullName, Encoding.ASCII);

			return new StringResult(contents);
		}
		catch (IOException)
		{
			return NullResult.Instance;
		}
	}
}
