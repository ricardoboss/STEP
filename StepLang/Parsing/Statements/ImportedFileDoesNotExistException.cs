using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;

namespace StepLang.Parsing.Statements;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
internal class ImportedFileDoesNotExistException : InterpreterException
{
    public ImportedFileDoesNotExistException(FileSystemInfo fileInfo) : base("Imported file does not exist: " + fileInfo.FullName)
    {
    }
}