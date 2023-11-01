namespace StepLang.Framework.Other;

public abstract class FileFunction : NativeFunction
{
    protected static FileInfo GetFileInfoFromPath(string path)
    {
        FileInfo fileInfo;
        if (Path.IsPathRooted(path))
            fileInfo = new(path);
        else
        {
            var currentDirectory = Directory.GetCurrentDirectory() ?? throw new InvalidOperationException();
            var resolvedPath = Path.Combine(currentDirectory, path);

            fileInfo = new(resolvedPath);
        }

        return fileInfo;
    }
}