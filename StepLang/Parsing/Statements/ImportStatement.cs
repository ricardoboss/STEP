using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

internal sealed class ImportStatement : Statement
{
    private readonly Token filePathToken;

    public ImportStatement(Token importToken, Token filePathToken) : base(StatementType.ImportStatement)
    {
        if (filePathToken.Type is not TokenType.LiteralString)
            throw new UnexpectedTokenException(filePathToken, TokenType.LiteralString);

        this.filePathToken = filePathToken;

        Location = importToken.Location;
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        FileInfo fileInfo;
        if (Location is null)
            fileInfo = new(filePathToken.Value);
        else if (Path.IsPathRooted(filePathToken.Value))
            fileInfo = new(filePathToken.Value);
        else
        {
            var currentDirectory = Path.GetDirectoryName(Location.File.FullName) ?? throw new InvalidOperationException();
            var resolvedPath = Path.Combine(currentDirectory, filePathToken.Value);

            fileInfo = new(resolvedPath);
        }

        if (!fileInfo.Exists)
            throw new ImportedFileDoesNotExistException(Location, fileInfo);

        if (fileInfo.FullName == Location?.File.FullName)
            throw new ImportedFileIsSelfException(Location, fileInfo);

        var fileContents = await File.ReadAllTextAsync(fileInfo.FullName, cancellationToken);

        var tokenizer = new Tokenizer();
        tokenizer.UpdateFile(fileInfo);
        tokenizer.Add(fileContents);
        var tokens = tokenizer.TokenizeAsync(cancellationToken);

        var parser = new StatementParser();
        parser.Add(tokens);
        var statements = parser.ParseAsync(cancellationToken);

        await interpreter.InterpretAsync(statements, cancellationToken);
    }
}