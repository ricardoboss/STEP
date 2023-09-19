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
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        return obj.GetType() == GetType() && Equals((ExpressionResult)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode() => (int)ResultType;

    /// <inheritdoc />
    public override string ToString() => ResultType.ToTypeName();

    public NumberResult ExpectNumber()
    {
        if (this is not NumberResult numberResult)
            throw new InvalidOperationException($"Expected a {ResultType.Number.ToTypeName()}, but got {ResultType.ToTypeName()}");

        return numberResult;
    }

    public NumberResult ExpectInteger()
    {
        var numberResult = ExpectNumber();

        if (!numberResult.IsInteger)
            throw new InvalidOperationException($"Expected an integer, but got {numberResult.Value}");

        return numberResult;
    }

    public StringResult ExpectString()
    {
        if (this is not StringResult stringResult)
            throw new InvalidOperationException($"Expected a {ResultType.Str.ToTypeName()}, but got {ResultType.ToTypeName()}");

        return stringResult;
    }

    public BoolResult ExpectBool()
    {
        if (this is not BoolResult boolResult)
            throw new InvalidOperationException($"Expected a {ResultType.Bool.ToTypeName()}, but got {ResultType.ToTypeName()}");

        return boolResult;
    }

    public ListResult ExpectList()
    {
        if (this is not ListResult listResult)
            throw new InvalidOperationException($"Expected a {ResultType.List.ToTypeName()}, but got {ResultType.ToTypeName()}");

        return listResult;
    }

    public MapResult ExpectMap()
    {
        if (this is not MapResult mapResult)
            throw new InvalidOperationException($"Expected a {ResultType.Map.ToTypeName()}, but got {ResultType.ToTypeName()}");

        return mapResult;
    }

    public FunctionResult ExpectFunction()
    {
        if (this is not FunctionResult functionResult)
            throw new InvalidOperationException($"Expected a {ResultType.Function.ToTypeName()}, but got {ResultType.ToTypeName()}");

        return functionResult;
    }

    public abstract ExpressionResult DeepClone();

    public ConstantExpression ToExpression() => new(this);
}