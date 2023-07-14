namespace HILFE.Interpreting;

public class Config
{
    public static Config FromEnvironment()
    {
        return new();
    }

    public static Config FromFile(FileInfo info)
    {
        return new();
    }
}