using Semver;

namespace StepLang.Libraries;

public record Dependency(string Name, SemVersionRange Version);
