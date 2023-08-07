namespace StepLang.Tokenizing;

public record TokenLocation(FileSystemInfo File, int Line, int Column);
