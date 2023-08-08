using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
internal sealed class ImportedFileDoesNotExistException : InterpreterException
{
    public ImportedFileDoesNotExistException(TokenLocation? importTokenLocation, FileSystemInfo fileInfo) : base(importTokenLocation, "Imported file does not exist: " + fileInfo.FullName)
    {
    }
}