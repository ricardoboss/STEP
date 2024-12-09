using StepLang.Tokenizing;

namespace StepLang.Interpreting;

internal sealed class ImportedFileDoesNotExistException(TokenLocation importTokenLocation, FileSystemInfo fileInfo)
	: ImportException(1,
		importTokenLocation, "Imported file does not exist: " + fileInfo.FullName,
		"Check the path of the imported file. If the path is absolute, make sure it is correct. If the path is relative, make sure it is relative to the file that is importing it.");
