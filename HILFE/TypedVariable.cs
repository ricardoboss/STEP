namespace HILFE;

public class TypedVariable
{
    public readonly string Identifier;
    public readonly string TypeName;
    public readonly dynamic? Value;
    public readonly Type? OriginalType;

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

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Identifier} ({TypeName}): {Value}";
    }
}