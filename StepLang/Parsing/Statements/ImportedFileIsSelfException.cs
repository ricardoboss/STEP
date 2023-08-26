using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

internal sealed class ImportedFileIsSelfException : ParserException
{
    public ImportedFileIsSelfException(TokenLocation location, FileSystemInfo fileInfo) : base(6, location, "Imported file imports itself: " + fileInfo.FullName, "Files cannot import themselves as this would cause an infinite loop. If you didn't intend to import this file, check the path of the imported file.")
    {
    }
}