using StepLang.Expressions.Results;
using StepLang.Framework.Other;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;

namespace StepLang.Tests.Framework.Other;

public class FileFunctionsTest
{
	[TestCase("Windows", @"C:\temp\test.txt")]
	[TestCase("Linux", "/tmp/test.txt")]
	[TestCase("macOS", "/tmp/test.txt")]
	public void TestFileFunctions(string platform, string filename)
	{
		if (!OperatingSystem.IsOSPlatform(platform))
		{
			return;
		}

		const string content = "Hello World";

		var interpreter = new Interpreter();

		var fileExistsFunction = new FileExistsFunction();
		var fileWriteFunction = new FileWriteFunction();
		var fileReadFunction = new FileReadFunction();
		var fileDeleteFunction = new FileDeleteFunction();

		var preWriteExistsResult = fileExistsFunction.Invoke(new(), interpreter,
			new List<ExpressionNode> { LiteralExpressionNode.FromString(filename) });

		var preWriteExistsBoolResult = AssertIsType<BoolResult>(preWriteExistsResult);
		Assert.That(preWriteExistsBoolResult.Value, Is.False);

		var writeArguments = new List<ExpressionNode>
		{
			LiteralExpressionNode.FromString(filename), LiteralExpressionNode.FromString(content),
		};

		var writeResult = fileWriteFunction.Invoke(new(), interpreter, writeArguments);

		var writeBoolResult = AssertIsType<BoolResult>(writeResult);
		Assert.That(writeBoolResult.Value, Is.True);

		var postWriteExistsResult = fileExistsFunction.Invoke(new(), interpreter,
			new List<ExpressionNode> { LiteralExpressionNode.FromString(filename) });

		var postWriteExistsBoolResult = AssertIsType<BoolResult>(postWriteExistsResult);
		Assert.That(postWriteExistsBoolResult.Value, Is.True);

		var firstReadResult = fileReadFunction.Invoke(new(), interpreter,
			new List<ExpressionNode> { LiteralExpressionNode.FromString(filename) });

		var firstReadStringResult = AssertIsType<StringResult>(firstReadResult);
		Assert.That(firstReadStringResult.Value, Is.EqualTo(content));

		var appendArguments = new List<ExpressionNode>
		{
			LiteralExpressionNode.FromString(filename),
			LiteralExpressionNode.FromString(content),
			LiteralExpressionNode.FromBoolean(true),
		};

		var appendResult = fileWriteFunction.Invoke(new(), interpreter, appendArguments);

		var appendBoolResult = AssertIsType<BoolResult>(appendResult);
		Assert.That(appendBoolResult.Value, Is.True);

		var secondReadResult = fileReadFunction.Invoke(new(), interpreter,
			new List<ExpressionNode> { LiteralExpressionNode.FromString(filename) });

		var secondReadStringResult = AssertIsType<StringResult>(secondReadResult);
		Assert.That(secondReadStringResult.Value, Is.EqualTo(content + content));

		var deleteResult = fileDeleteFunction.Invoke(new(), interpreter,
			new List<ExpressionNode> { LiteralExpressionNode.FromString(filename) });

		var deleteBoolResult = AssertIsType<BoolResult>(deleteResult);
		Assert.That(deleteBoolResult.Value, Is.True);
	}

	private static T AssertIsType<T>(object? value)
	{
		Assert.That(value, Is.TypeOf<T>());
		return (T)value!;
	}
}
