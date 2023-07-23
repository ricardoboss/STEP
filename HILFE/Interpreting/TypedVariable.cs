namespace HILFE.Interpreting;

public class TypedVariable
{
    public string Identifier { get; }
    public string TypeName { get; }
    public dynamic? Value { get; private set; }
    public Type? OriginalType { get; }

    public TypedVariable(string identifier, string typeName, dynamic? value)
    {
        Identifier = identifier;
        TypeName = typeName;
        Value = value;
        OriginalType = value?.GetType();
    }

    public TCast? TryConvert<TCast>() where TCast : struct
    {
        if (Value is null)
            return null;

        var targetType = typeof(TCast);

        if (OriginalType == targetType)
            return (TCast?)(object)Value;

        return null;
    }

    public void Assign(dynamic? newValue)
    {
        // TODO: type checks
        Value = newValue;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Identifier} ({TypeName}): {Value}";
    }
}