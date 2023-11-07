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

    public override bool Equals(object? obj) => obj is ExpressionResult result && EqualsInternal(result);

    protected override bool EqualsInternal(ExpressionResult other) => other is StringResult stringResult && string.Equals(Value, stringResult.Value, StringComparison.Ordinal);

    public override int GetHashCode() => base.GetHashCode();

    public override StringResult DeepClone()
    {
        return new(Value);
    }

    /// <inheritdoc />
    public override string ToString() => $"\"{Value}\"";

    public static implicit operator StringResult(string value) => new(value);
    public static implicit operator string(StringResult result) => result.Value;

    public static StringResult operator +(StringResult left, StringResult right) => new(left.Value + right.Value);

    public static StringResult operator +(StringResult left, NumberResult right) => new(left.Value + right.Value);

    public static StringResult operator +(NumberResult left, StringResult right) => new(left.Value + right.Value);

    public static BoolResult operator ==(StringResult left, StringResult right) => new(string.Equals(left.Value, right.Value, StringComparison.Ordinal));

    public static BoolResult operator !=(StringResult left, StringResult right) => new(!string.Equals(left.Value, right.Value, StringComparison.Ordinal));

    public static StringResult FromString(string value) => value;

    public static StringResult Add(StringResult left, StringResult right) => left + right;

    public static StringResult Add(StringResult left, NumberResult right) => left + right;

    public static StringResult Add(NumberResult left, StringResult right) => left + right;
}