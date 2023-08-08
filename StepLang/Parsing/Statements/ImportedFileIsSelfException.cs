using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
internal class ImportedFileIsSelfException : InterpreterException
{
    public ImportedFileIsSelfException(TokenLocation location, FileSystemInfo fileInfo) : base(location, "Imported file imports itself: " + fileInfo.FullName)
    {
    }
}