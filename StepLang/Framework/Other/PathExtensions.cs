using StepLang.Tokenizing;

namespace StepLang.Framework.Other;

/// <summary>
/// Extension methods for <see cref="TokenLocation"/>s.
/// </summary>
public static class PathExtensions
{
    /// <summary>
    /// Gets a <see cref="FileInfo"/> from a path relative to the <see cref="TokenLocation"/>.
    /// </summary>
    /// <param name="location">The <see cref="TokenLocation"/> to base the path on.</param>
    /// <param name="path">The path to get the <see cref="FileInfo"/> from.</param>
    /// <returns>The <see cref="FileInfo"/> from the path.</returns>
    /// <exception cref="InvalidOperationException">When the current directory could not be retrieved.</exception>
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
