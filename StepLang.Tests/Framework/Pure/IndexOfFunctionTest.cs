using StepLang.Expressions.Results;
using StepLang.Framework.Pure;

namespace StepLang.Tests.Framework.Pure;

public class IndexOfFunctionTest
{
    [Theory]
    [InlineData("", "", -1)]
    [InlineData("", "a", -1)]
    [InlineData("a", "", 0)]
    [InlineData("a", "a", 0)]
    [InlineData("a", "b", -1)]
    [InlineData("ab", "a", 0)]
    [InlineData("ab", "b", 1)]
    [InlineData("a🤷‍♂️b", "a", 0)]
    [InlineData("a🤷‍♂️b", "🤷‍♂️", 1)]
    [InlineData("a🤷‍♂️b", "b", 2)]
    public void TestIndexOfString(string subject, string value, int expected)
    {
        var result = IndexOfFunction.GetResult(new StringResult(subject), new StringResult(value));

        Assert.Equal(expected, result.ExpectNumber().Value);
    }
}