using System.Text.Json.Serialization;
using StepLang.Framework.Conversion;

namespace StepLang.Expressions.Results;

[JsonConverter(typeof(ExpressionResultJsonConverter))]
public abstract class ExpressionResult : IEquatable<ExpressionResult>
{
    public static ExpressionResult DefaultFor(ResultType resultType)
    {
        return resultType switch
        {
            ResultType.Void => VoidResult.Instance,
            ResultType.Str => StringResult.Empty,
            ResultType.Number => NumberResult.Zero,
            ResultType.Bool => BoolResult.False,
            ResultType.List => ListResult.Empty,
            ResultType.Map => MapResult.Empty,
            ResultType.Function => FunctionResult.VoidFunction,
            ResultType.Null => NullResult.Instance,
            _ => throw new ArgumentOutOfRangeException(nameof(resultType), resultType, null),
        };
    }

    protected ExpressionResult(ResultType resultType) => ResultType = resultType;

    public ResultType ResultType { get; }

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as ExpressionResult);

    /// <inheritdoc />
    public bool Equals(ExpressionResult? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (ResultType != other.ResultType)
            return false;

        return EqualsInternal(other);
    }

    protected abstract bool EqualsInternal(ExpressionResult other);

    /// <inheritdoc />
    public override int GetHashCode() => (int)ResultType;

    /// <inheritdoc />
    public override string ToString() => ResultType.ToTypeName();

    public abstract ExpressionResult DeepClone();

    public bool IsTruthy()
    {
        return this switch
        {
            BoolResult { Value: true } => true,
            _ => false,
        };
    }
}