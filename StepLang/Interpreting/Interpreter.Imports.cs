using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public partial class Interpreter : IImportNodeVisitor
{
    public void Visit(ImportNode importNode)
    {
        var pathToken = importNode.PathToken;

        FileInfo importedFile;
        if (pathToken.Location.DocumentUri is null || Path.IsPathRooted(pathToken.StringValue))
            importedFile = new FileInfo(pathToken.StringValue);
        else
        {
            var file = pathToken.Location.File;
            var currentDirectory = file is not null ? Path.GetDirectoryName(file.FullName) : null;
            if (currentDirectory is null)
                throw new IOException("Could not get current directory.");

            var resolvedPath = Path.Combine(currentDirectory, pathToken.StringValue);

            importedFile = new FileInfo(resolvedPath);
        }

        if (!importedFile.Exists)
            throw new ImportedFileDoesNotExistException(pathToken.Location, importedFile);

        if (importedFile.FullName == pathToken.Location.File?.FullName)
            throw new ImportedFileIsSelfException(pathToken.Location, importedFile);

        var source = CharacterSource.FromFile(importedFile);
        var tokenizer = new Tokenizer(source);
        var tokens = tokenizer.Tokenize();

        var parser = new Parser(tokens);
        var root = parser.ParseRoot();

        Visit(root);
    }
}