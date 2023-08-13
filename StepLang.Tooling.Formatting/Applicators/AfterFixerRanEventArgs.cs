using StepLang.Formatters.Fixers;

namespace StepLang.Formatters.Applicators;

public record AfterFixerRanEventArgs(FileInfo File, IFixer Fixer);