using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Statements;

internal sealed class ImportedFileDoesNotExistException : ParserException
{
    public ImportedFileDoesNotExistException(TokenLocation? importTokenLocation, FileSystemInfo fileInfo) : base(5, importTokenLocation, "Imported file does not exist: " + fileInfo.FullName, "Check the path of the imported file. If the path is absolute, make sure it is correct. If the path is relative, make sure it is relative to the file that is importing it.")
    {
    }
}