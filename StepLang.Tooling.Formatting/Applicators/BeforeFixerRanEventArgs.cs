using StepLang.Formatters.Fixers;

namespace StepLang.Formatters.Applicators;

public record BeforeFixerRanEventArgs(FileInfo File, IFixer Fixer);