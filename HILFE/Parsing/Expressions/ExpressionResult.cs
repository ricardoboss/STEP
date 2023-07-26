using HILFE.Interpreting;

namespace HILFE.Parsing.Expressions;

public record ExpressionResult(string ValueType, dynamic? Value = null, bool IsVoid = false)
{
    public static ExpressionResult Void { get; } = new("void", IsVoid: true);

    public static ExpressionResult Null { get; } = new("null");

    public static ExpressionResult True { get; } = new("bool", true);

    public static ExpressionResult False { get; } = new("bool", false);
    
    public static ExpressionResult Number(double value) => new("number", value);
    
    public static ExpressionResult String(string value) => new("string", value);

    public static ExpressionResult Bool(bool value) => new("bool", value);

    public static ExpressionResult Function(FunctionDefinition definition) => new("function", definition);

    public static ExpressionResult Array(IEnumerable<ExpressionResult> items) => new("array", items);

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

    public string ExpectString()
    {
        if (ValueType is not "string")
            throw new InvalidResultTypeException("string", ValueType);

        return Value?.ToString() ?? string.Empty;
    }

    public bool ExpectBool()
    {
        if (ValueType is not "bool")
            throw new InvalidResultTypeException("bool", ValueType);

        return Value is true;
    }

    public FunctionDefinition ExpectFunction()
    {
        if (ValueType is not "function" || Value is not FunctionDefinition definition)
            throw new InvalidResultTypeException("function", ValueType);

        return definition;
    }

    public void ThrowIfVoid(string? expected = null)
    {
        if (ValueType is "void" || IsVoid)
            throw new InvalidResultTypeException(expected ?? "non-void", "void");
    }

    public void ThrowIfNotVoid()
    {
        if (ValueType is not "void" || !IsVoid)
            throw new InvalidResultTypeException("void", ValueType);
    }
}