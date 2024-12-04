using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace StepLang.LSP;

internal class FoldingRangeHandler : IFoldingRangeHandler
{
    public FoldingRangeRegistrationOptions GetRegistrationOptions() =>
        new()
        {
            DocumentSelector = TextDocumentSelector.ForLanguage("STEP"),
        };

    public Task<Container<FoldingRange>?> Handle(
        FoldingRangeRequestParam request,
        CancellationToken cancellationToken
    ) =>
        Task.FromResult<Container<FoldingRange>?>(
            new(
                new FoldingRange {
                    StartLine = 10,
                    EndLine = 20,
                    Kind = FoldingRangeKind.Region,
                    EndCharacter = 0,
                    StartCharacter = 0,
                }
            )
        );

    public FoldingRangeRegistrationOptions GetRegistrationOptions(FoldingRangeCapability capability, ClientCapabilities clientCapabilities) => new()
    {
        DocumentSelector = TextDocumentSelector.ForLanguage("STEP"),
    };
}