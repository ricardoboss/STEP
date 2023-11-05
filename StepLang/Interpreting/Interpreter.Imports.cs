using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public partial class Interpreter : IImportNodeVisitor
{
    public void Visit(ImportNode importNode)
    {
        var pathToken = importNode.PathToken;

        FileInfo importedFile;
        if (pathToken.Location.File is null || Path.IsPathRooted(pathToken.StringValue))
            importedFile = new(pathToken.StringValue);
        else
        {
            var baseFile = pathToken.Location.File;
            var currentDirectory = Path.GetDirectoryName(baseFile.FullName);
            if (currentDirectory is null)
                throw new IOException("Could not get current directory.");

            var resolvedPath = Path.Combine(currentDirectory, pathToken.StringValue);

            importedFile = new(resolvedPath);
        }

        if (!importedFile.Exists)
            throw new ImportedFileDoesNotExistException(pathToken.Location, importedFile);

        if (importedFile.FullName == pathToken.Location.File?.FullName)
            throw new ImportedFileIsSelfException(pathToken.Location, importedFile);

        var fileContents = File.ReadAllText(importedFile.FullName);

        var tokenizer = new Tokenizer();
        tokenizer.UpdateFile(importedFile);
        tokenizer.Add(fileContents);
        var tokens = tokenizer.Tokenize();

        var parser = new Parser(tokens);
        var root = parser.ParseRoot();

        Run(root);
    }
}