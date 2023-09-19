using System.Runtime.InteropServices;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Framework.Other;
using StepLang.Interpreting;

namespace StepLang.Tests.Framework.Other;

public class FileFunctionsTest
{
    [SkippableTheory]
    [InlineData("Windows", @"C:\temp\test.txt", "Hello World")]
    [InlineData("Linux", "/tmp/test.txt", "Hello World")]
    [InlineData("OSX", "/tmp/test.txt", "Hello World")]
    public async Task TestFileFunctions(string platform, string filename, string content)
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Create(platform)), $"Test only for {platform}");

        var interpreter = new Interpreter();

        var fileExistsFunction = new FileExistsFunction();
        var fileWriteFunction = new FileWriteFunction();
        var fileReadFunction = new FileReadFunction();
        var fileDeleteFunction = new FileDeleteFunction();

        var preWriteExistsResult = await fileExistsFunction.EvaluateAsync(interpreter, new List<Expression> { LiteralExpression.Str(filename) });

        Assert.IsType<BoolResult>(preWriteExistsResult);
        Assert.False(preWriteExistsResult.ExpectBool().Value);

        var writeResult = await fileWriteFunction.EvaluateAsync(interpreter, new List<Expression> { LiteralExpression.Str(filename), LiteralExpression.Str(content) });

        Assert.IsType<BoolResult>(writeResult);
        Assert.True(writeResult.ExpectBool().Value);

        var postWriteExistsResult = await fileExistsFunction.EvaluateAsync(interpreter, new List<Expression> { LiteralExpression.Str(filename) });

        Assert.IsType<BoolResult>(postWriteExistsResult);
        Assert.True(postWriteExistsResult.ExpectBool().Value);

        var firstReadResult = await fileReadFunction.EvaluateAsync(interpreter, new List<Expression> { LiteralExpression.Str(filename) });

        Assert.IsType<StringResult>(firstReadResult);
        Assert.Equal(content, firstReadResult.ExpectString().Value);

        var appendResult = await fileWriteFunction.EvaluateAsync(interpreter, new List<Expression> { LiteralExpression.Str(filename), LiteralExpression.Str(content), LiteralExpression.True });

        Assert.IsType<BoolResult>(appendResult);
        Assert.True(appendResult.ExpectBool().Value);

        var secondReadResult = await fileReadFunction.EvaluateAsync(interpreter, new List<Expression> { LiteralExpression.Str(filename) });

        Assert.IsType<StringResult>(secondReadResult);
        Assert.Equal(content + content, secondReadResult.ExpectString().Value);

        var deleteResult = await fileDeleteFunction.EvaluateAsync(interpreter, new List<Expression> { LiteralExpression.Str(filename) });

        Assert.IsType<BoolResult>(deleteResult);
        Assert.True(deleteResult.ExpectBool().Value);
    }
}