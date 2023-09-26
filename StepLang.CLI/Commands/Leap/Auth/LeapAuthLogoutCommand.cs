using System.Diagnostics.CodeAnalysis;
using Leap.Client;
using Spectre.Console.Cli;
using StepLang.CLI.Commands.Settings;

namespace StepLang.CLI.Commands.Leap.Auth;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LeapAuthLogoutCommand : AsyncCommand<HiddenGlobalCommandSettings>
{
    private readonly LeapApiCredentialManager credentialManager;

    public LeapAuthLogoutCommand(LeapApiCredentialManager credentialManager)
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