namespace HILFE.Parsing.Expressions;

public record ExpressionResult(string ValueType, dynamic? Value = null, bool IsVoid = false)
{
    /// <inheritdoc />
    public override string ToString()
    {
        var display = IsVoid ? "<void>" : Value is not null ? $"({ValueType}) {Value}" : "<null>";

        return $"<{nameof(ExpressionResult)}: {display}>";
    }
}
