using StepLang.Tokenizing;

namespace StepLang.Framework.Other;

public static class PathExtensions
{
    public static FileInfo GetFileInfoFromPath(this TokenLocation location, string path)
    {
        FileInfo fileInfo;
        if (Path.IsPathRooted(path))
            fileInfo = new(path);
        else
        {
            string? currentDirectory;
            if (location.File is { } locationFile)
                currentDirectory = Path.GetDirectoryName(locationFile.FullName);
            else
                currentDirectory = Directory.GetCurrentDirectory();

            if (currentDirectory is null)
                throw new InvalidOperationException("Could not get current directory.");

            var resolvedPath = Path.Combine(currentDirectory, path);

            fileInfo = new(resolvedPath);
        }

        return fileInfo;
    }
}
