using StepLang.Tooling.Formatting.Fixers;

namespace StepLang.Tooling.Formatting.Applicators;

public record AfterFixerRanEventArgs(FileInfo File, IFixer Fixer);