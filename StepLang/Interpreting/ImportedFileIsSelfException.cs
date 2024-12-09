using StepLang.Tokenizing;

namespace StepLang.Interpreting;

internal sealed class ImportedFileIsSelfException(TokenLocation location, FileSystemInfo fileInfo) : ImportException(2,
	location,
	"Imported file imports itself: " + fileInfo.FullName,
	"Files cannot import themselves as this would cause an infinite loop. If you didn't intend to import this file, check the path of the imported file.");
