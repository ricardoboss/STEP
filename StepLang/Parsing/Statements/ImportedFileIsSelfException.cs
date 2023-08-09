using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

internal sealed class ImportedFileIsSelfException : InterpreterException
{
    public ImportedFileIsSelfException(TokenLocation location, FileSystemInfo fileInfo) : base(location, "Imported file imports itself: " + fileInfo.FullName, "Files cannot import themselves as this would cause an infinite loop. If you didn't intend to import this file, check the path of the imported file.")
    {
    }
}