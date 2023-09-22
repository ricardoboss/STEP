using StepLang.Expressions.Results;

namespace StepLang.Interpreting;

public class InvalidResultTypeException : IncompatibleTypesException
{
    private static string ToExpectation(IReadOnlyList<ResultType> expected) => expected.Count switch
    {
        0 => "nothing",
        1 => $"a {expected[0].ToTypeName()}",
        _ => $"one of {string.Join(", ", expected.Select(e => e.ToTypeName()))}",
    };

    private static string BuildMessage(ResultType got, IReadOnlyList<ResultType> expected)
    {
        var expectation = ToExpectation(expected);

        return $"Invalid result type: expected {expectation}, got {got.ToTypeName()}";
    }

    private static string BuildMessage(ExpressionResult got, IReadOnlyList<ResultType> expected)
    {
        var expectation = ToExpectation(expected);
        var gotString = got.ResultType switch
        {
            ResultType.Void => "void",
            ResultType.Null => "null",
            ResultType.Function => "a function",
            ResultType.Str => $"a string (\"{got}\")",
            _ => $"a {got.ResultType.ToTypeName()} ({got})",
        };

        return $"Invalid result type: expected {expectation}, got {gotString}";
    }

    public InvalidResultTypeException(ResultType got, params ResultType[] expected) : this(BuildMessage(got, expected))
    {
    }

    public InvalidResultTypeException(ExpressionResult got, params ResultType[] expected) : this(BuildMessage(got, expected))
    {
    }

    private InvalidResultTypeException(string message) : base(4, null, message, "An expression evaluated to an unexpected type. Check what types of expression results are allowed in the current context.")
    {
    }
}