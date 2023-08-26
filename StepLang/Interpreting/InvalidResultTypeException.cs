using StepLang.Parsing.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class InvalidResultTypeException : IncompatibleTypesException
{
    private static string BuildMessage(ResultType got, ResultType[] expected)
    {
        var expectation = expected.Length switch
        {
            0 => "nothing",
            1 => $"a {expected[0].ToTypeName()}",
            _ => $"one of {string.Join(", ", expected.Select(e => e.ToTypeName()))}",
        };

        return $"Invalid result type: expected {expectation}, got {got.ToTypeName()}";
    }

    public InvalidResultTypeException(ResultType got, params ResultType[] expected) : base(5, null, BuildMessage(got, expected), "An expression evaluated to an unexpected type. Check what types of expression results are allowed in the current context.")
    {
    }
}