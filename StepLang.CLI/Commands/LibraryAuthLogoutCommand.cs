using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;
using Leap.Client;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LibraryAuthLogoutCommand : AsyncCommand<HiddenGlobalCommandSettings>
{
    private readonly LibApiCredentialManager credentialManager;

    public LibraryAuthLogoutCommand(LibApiCredentialManager credentialManager)
    {
        this.credentialManager = credentialManager;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, HiddenGlobalCommandSettings settings)
    {
        credentialManager.DestroyCredentials();

        await Console.Out.WriteLineAsync("Credentials deleted.");

        return 0;
    }
}