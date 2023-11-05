using System.Text;

namespace StepLang.Expressions.Results;

public class StringResult : ComparableValueExpressionResult<string>
{
    public static StringResult Empty => new(string.Empty);

    /// <inheritdoc />
    public StringResult(string value) : base(ResultType.Str, value)
    {
    }

    protected override int CompareToInternal(ComparableValueExpressionResult<string> other)
    {
        var normalizedA = Value.Normalize(NormalizationForm.FormD);
        var normalizedB = other.Value.Normalize(NormalizationForm.FormD);

        return string.Compare(normalizedA, normalizedB, StringComparison.Ordinal);
    }

    protected override bool EqualsInternal(ExpressionResult other)
    {
        return other is StringResult stringResult && string.Equals(Value, stringResult.Value, StringComparison.Ordinal);
    }

    public override StringResult DeepClone()
    {
        return new(Value);
    }

    /// <inheritdoc />
    public override string ToString() => $"\"{Value}\"";

    public static implicit operator StringResult(string value) => new(value);

    public static StringResult operator +(StringResult left, StringResult right) => new(left.Value + right.Value);

    public static StringResult operator +(StringResult left, NumberResult right) => new(left.Value + right.Value);

    public static StringResult operator +(NumberResult left, StringResult right) => new(left.Value + right.Value);

    public static BoolResult operator ==(StringResult left, StringResult right) => new(string.Equals(left.Value, right.Value, StringComparison.Ordinal));

    public static BoolResult operator !=(StringResult left, StringResult right) => new(!string.Equals(left.Value, right.Value, StringComparison.Ordinal));
}