using StepLang.Tooling.Formatting.Fixers;

namespace StepLang.Tooling.Formatting.Applicators;

public record BeforeFixerRanEventArgs(FileInfo File, IFixer Fixer);