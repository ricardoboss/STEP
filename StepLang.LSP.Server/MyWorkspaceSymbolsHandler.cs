using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Progress;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.WorkDone;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;

namespace StepLang.LSP.Server;

internal class MyWorkspaceSymbolsHandler : IWorkspaceSymbolsHandler
{
    private readonly IServerWorkDoneManager _serverWorkDoneManager;
    private readonly IProgressManager _progressManager;
    private readonly ILogger<MyWorkspaceSymbolsHandler> _logger;

    public MyWorkspaceSymbolsHandler(IServerWorkDoneManager serverWorkDoneManager, IProgressManager progressManager, ILogger<MyWorkspaceSymbolsHandler> logger)
    {
        _serverWorkDoneManager = serverWorkDoneManager;
        _progressManager = progressManager;
        _logger = logger;
    }

    public async Task<Container<SymbolInformation>?> Handle(
        WorkspaceSymbolParams request,
        CancellationToken cancellationToken
    )
    {
        using var reporter = _serverWorkDoneManager.For(
            request, new()
            {
                Cancellable = true,
                Message = "This might take a while...",
                Title = "Some long task....",
                Percentage = 0,
            }
        );

        using var partialResults = _progressManager.For(request, cancellationToken);
        // await Task.Delay(200, cancellationToken).ConfigureAwait(false);

        reporter.OnNext(
            new WorkDoneProgressReport
            {
                Cancellable = true,
                Percentage = 20,
            }
        );

        // await Task.Delay(500, cancellationToken).ConfigureAwait(false);

        reporter.OnNext(
            new WorkDoneProgressReport
            {
                Cancellable = true,
                Percentage = 40,
            }
        );

        // await Task.Delay(500, cancellationToken).ConfigureAwait(false);

        reporter.OnNext(
            new WorkDoneProgressReport
            {
                Cancellable = true,
                Percentage = 50,
            }
        );

        // await Task.Delay(50, cancellationToken).ConfigureAwait(false);

        partialResults.OnNext(
            new []
            {
                new SymbolInformation
                {
                    ContainerName = "Partial Container",
                    Deprecated = true,
                    Kind = SymbolKind.Constant,
                    Location = new()
                    {
                        Range = new(
                            new(2, 1),
                            new(2, 10)
                        ),
                    },
                    Name = "Partial name",
                },
            }
        );

        reporter.OnNext(
            new WorkDoneProgressReport
            {
                Cancellable = true,
                Percentage = 70,
            }
        );

        // await Task.Delay(50, cancellationToken).ConfigureAwait(false);

        reporter.OnNext(
            new WorkDoneProgressReport
            {
                Cancellable = true,
                Percentage = 90,
            }
        );

        partialResults.OnCompleted();
        return Array.Empty<SymbolInformation>();
    }

    public WorkspaceSymbolRegistrationOptions GetRegistrationOptions(WorkspaceSymbolCapability capability, ClientCapabilities clientCapabilities) => new();
}