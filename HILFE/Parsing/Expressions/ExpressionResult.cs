namespace HILFE.Parsing.Expressions;

public record ExpressionResult(string ValueType, dynamic? Value = null, bool IsVoid = false)
{
    public static ExpressionResult Void { get; } = new("void", IsVoid: true);

    public static ExpressionResult Null { get; } = new("null");

    /// <inheritdoc />
    public virtual bool Equals(ExpressionResult? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return ValueType == other.ValueType && Value == other.Value && IsVoid == other.IsVoid;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(ValueType, Value, IsVoid);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var display = IsVoid ? "<void>" : Value is not null ? $"({ValueType}) {Value}" : "<null>";

        return $"<{nameof(ExpressionResult)}: {display}>";
    }
}
