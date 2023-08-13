using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

internal sealed class ImportedFileDoesNotExistException : InterpreterException
{
    public ImportedFileDoesNotExistException(TokenLocation? importTokenLocation, FileSystemInfo fileInfo) : base(5, importTokenLocation, "Imported file does not exist: " + fileInfo.FullName, "Check the path of the imported file. If the path is absolute, make sure it is correct. If the path is relative, make sure it is relative to the file that is importing it.")
    {
    }
}