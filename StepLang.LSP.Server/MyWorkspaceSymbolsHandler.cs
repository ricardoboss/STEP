using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Progress;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.WorkDone;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace StepLang.LSP.Server;

internal class MyWorkspaceSymbolsHandler(
    IServerWorkDoneManager serverWorkDoneManager,
    IProgressManager progressManager,
    ILogger<MyWorkspaceSymbolsHandler> logger)
    : IWorkspaceSymbolsHandler
{
    public async Task<Container<WorkspaceSymbol>?> Handle(WorkspaceSymbolParams request, CancellationToken cancellationToken)
    {
        using var reporter = serverWorkDoneManager.For(
            request, new()
            {
                Cancellable = true,
                Message = "This might take a while...",
                Title = "Some long task....",
                Percentage = 0,
            }
        );

        using var partialResults = progressManager.For(request, cancellationToken);
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
                new WorkspaceSymbol()
                {
                    ContainerName = "Partial Container",
                    Kind = SymbolKind.Constant,
                    Location = new LocationOrFileLocation(new Location()
                    {
                        Uri = new Uri("file:///c:/users/me/file.txt"),
                        Range = new Range
                        {
                            Start = new Position
                            {
                                Line = 1,
                                Character = 2,
                            },
                            End = new Position
                            {
                                Line = 3,
                                Character = 4,
                            },
                        },
                    }),
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

        return Array.Empty<WorkspaceSymbol>();
    }

    public WorkspaceSymbolRegistrationOptions GetRegistrationOptions(WorkspaceSymbolCapability capability, ClientCapabilities clientCapabilities) => new();
}