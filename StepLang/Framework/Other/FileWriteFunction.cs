using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;
using System.Text;

namespace StepLang.Framework.Other;

public class FileWriteFunction : GenericFunction<StringResult, StringResult, BoolResult>
{
	public const string Identifier = "fileWrite";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyString, "path"),
		new(OnlyString, "content"),
		new(OnlyBool, "append", LiteralExpressionNode.FromBoolean(false)),
	];

	protected override BoolResult Invoke(TokenLocation callLocation, IInterpreter interpreter, StringResult argument1,
		StringResult argument2, BoolResult argument3)
	{
		var path = argument1.Value;
		var content = argument2.Value;
		var append = argument3.Value;

		var info = callLocation.GetFileInfoFromPath(path);

		try
		{
			if (append)
			{
				File.AppendAllText(info.FullName, content, Encoding.ASCII);
			}
			else
			{
				var directory = info.DirectoryName;
				if (!string.IsNullOrEmpty(directory))
				{
					_ = Directory.CreateDirectory(directory);
				}

				File.WriteAllText(info.FullName, content, Encoding.ASCII);
			}
		}
		catch (Exception e) when (e is IOException or SystemException)
		{
			if (interpreter.DebugOut is { } debugOut)
			{
				debugOut.WriteLine($"Failed to write to file '{path}': {e.Message}");
			}

			return false;
		}

		return true;
	}
}
