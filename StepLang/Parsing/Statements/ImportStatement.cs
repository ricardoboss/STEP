using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

internal class ImportStatement : Statement
{
    private readonly Token filePathToken;

    public ImportStatement(Token filePathToken) : base(StatementType.ImportStatement)
    {
        if (filePathToken.Type is not TokenType.LiteralString)
            throw new UnexpectedTokenException(filePathToken, TokenType.LiteralString);

        this.filePathToken = filePathToken;
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var fileInfo = new FileInfo(filePathToken.Value);

        if (!fileInfo.Exists)
            throw new ImportedFileDoesNotExistException(fileInfo);

        var fileContents = await File.ReadAllTextAsync(fileInfo.FullName, cancellationToken);

        var tokenizer = new Tokenizer();
        tokenizer.Add(fileContents);
        var tokens = tokenizer.TokenizeAsync(cancellationToken);

        var parser = new StatementParser();
        parser.Add(tokens);
        var statements = parser.ParseAsync(cancellationToken);

        await interpreter.InterpretAsync(statements, cancellationToken);
    }
}