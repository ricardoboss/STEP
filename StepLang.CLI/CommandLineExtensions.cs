using System.CommandLine;

namespace StepLang.CLI;

internal static class CommandLineExtensions
{
    public static Argument<FileInfo> WithStepExtensionOnly(this Argument<FileInfo> argument)
    {
        argument.AddValidator(result =>
        {
            var file = result.GetValueForArgument(argument);
            if (file.Extension != ".step")
            {
                result.ErrorMessage = $"The file '{file.FullName}' is not a .step-file";
            }
        });

        return argument;
    }
}