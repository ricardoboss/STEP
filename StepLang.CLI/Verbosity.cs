namespace StepLang.CLI;

public enum Verbosity
{
    Quiet,
    Normal,
    Verbose,
}

public static class VerbosityExtensions
{
    public static bool IsAtLeast(this Verbosity verbosity, Verbosity minimum) => verbosity >= minimum;

    public static bool IsAtMost(this Verbosity verbosity, Verbosity maximum) => verbosity <= maximum;
}