namespace HILFE.Parsing;

public record ExpressionResult(dynamic? Value = null, bool IsVoid = false)
{
    /// <inheritdoc />
    public override string ToString()
    {
        var display = IsVoid ? "<void>" : Value ?? "<null>";

        return $"<{nameof(ExpressionResult)}: {display}>";
    }
}
