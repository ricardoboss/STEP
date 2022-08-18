namespace HILFE;

public class TypedVariable
{
    public readonly string Name;
    public readonly dynamic? Value;
    public readonly Type OriginalType;

    public TypedVariable(string name, dynamic? value)
    {
        Name = name;
        Value = value;
        OriginalType = value.GetType();
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
}