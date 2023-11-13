using System.Runtime.InteropServices;
using StepLang.Expressions.Results;
using StepLang.Framework.Other;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Tests.Framework.Other;

public class FileFunctionsTest
{
    [SkippableTheory]
    [InlineData("Windows", @"C:\temp\test.txt")]
    [InlineData("Linux", "/tmp/test.txt")]
    [InlineData("OSX", "/tmp/test.txt")]
    public void TestFileFunctions(string platform, string filename)
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Create(platform)), $"Test only for {platform}");

        const string content = "Hello World";

        var debugOut = new StringWriter();
        var interpreter = new Interpreter(debugOut: debugOut);

        var fileExistsFunction = new FileExistsFunction();
        var fileWriteFunction = new FileWriteFunction();
        var fileReadFunction = new FileReadFunction();
        var fileDeleteFunction = new FileDeleteFunction();

        var preWriteExistsResult = fileExistsFunction.Invoke(new(), interpreter, new List<ExpressionNode> { LiteralExpressionNode.FromString(filename) });

        Assert.IsType<BoolResult>(preWriteExistsResult);
        Assert.False(((BoolResult)preWriteExistsResult).Value);

        var writeResult = fileWriteFunction.Invoke(new(), interpreter, new List<ExpressionNode> { LiteralExpressionNode.FromString(filename), LiteralExpressionNode.FromString(content) });

        Assert.IsType<BoolResult>(writeResult);
        Assert.True(((BoolResult)writeResult).Value);

        var postWriteExistsResult = fileExistsFunction.Invoke(new(), interpreter, new List<ExpressionNode> { LiteralExpressionNode.FromString(filename) });

        Assert.IsType<BoolResult>(postWriteExistsResult);
        Assert.True(((BoolResult)postWriteExistsResult).Value);

        var firstReadResult = fileReadFunction.Invoke(new(), interpreter, new List<ExpressionNode> { LiteralExpressionNode.FromString(filename) });

        Assert.IsType<StringResult>(firstReadResult);
        Assert.Equal(content, ((StringResult)firstReadResult).Value);

        var appendResult = fileWriteFunction.Invoke(new(), interpreter, new List<ExpressionNode> { LiteralExpressionNode.FromString(filename), LiteralExpressionNode.FromString(content), LiteralExpressionNode.FromBoolean(true) });

        Assert.IsType<BoolResult>(appendResult);
        Assert.True(((BoolResult)appendResult).Value);

        var secondReadResult = fileReadFunction.Invoke(new(), interpreter, new List<ExpressionNode> { LiteralExpressionNode.FromString(filename) });

        Assert.IsType<StringResult>(secondReadResult);
        Assert.Equal(content + content, ((StringResult)secondReadResult).Value);

        var deleteResult = fileDeleteFunction.Invoke(new(), interpreter, new List<ExpressionNode> { LiteralExpressionNode.FromString(filename) });

        Assert.IsType<BoolResult>(deleteResult);
        Assert.True(((BoolResult)deleteResult).Value);
    }
}