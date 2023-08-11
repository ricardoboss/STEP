using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace StepLang.LSP.Server;

internal class MyDocumentSymbolHandler : IDocumentSymbolHandler
{
    public async Task<SymbolInformationOrDocumentSymbolContainer> Handle(
        DocumentSymbolParams request,
        CancellationToken cancellationToken
    )
    {
        // you would normally get this from a common source that is managed by current open editor, current active editor, etc.
        var content = await File.ReadAllTextAsync(DocumentUri.GetFileSystemPath(request) ?? throw new InvalidOperationException(), cancellationToken).ConfigureAwait(false);
        var lines = content.Split('\n');
        var symbols = new List<SymbolInformationOrDocumentSymbol>();
        for (var lineIndex = 0; lineIndex < lines.Length; lineIndex++)
        {
            var line = lines[lineIndex];
            var parts = line.Split(' ', '.', '(', ')', '{', '}', '[', ']', ';');
            var currentCharacter = 0;
            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                {
                    currentCharacter += part.Length + 1;
                    continue;
                }

                symbols.Add(
                    new DocumentSymbol
                    {
                        Detail = part,
                        Deprecated = true,
                        Kind = SymbolKind.Field,
                        Tags = new []
                        {
                            SymbolTag.Deprecated,
                        },
                        Range = new(
                            new(lineIndex, currentCharacter),
                            new(lineIndex, currentCharacter + part.Length)
                        ),
                        SelectionRange =
                            new(
                                new(lineIndex, currentCharacter),
                                new(lineIndex, currentCharacter + part.Length)
                            ),
                        Name = part,
                    }
                );

                currentCharacter += part.Length + 1;
            }
        }

        // await Task.Delay(2000, cancellationToken);
        return symbols;
    }

    public DocumentSymbolRegistrationOptions GetRegistrationOptions(DocumentSymbolCapability capability, ClientCapabilities clientCapabilities) => new()
    {
        DocumentSelector = DocumentSelector.ForLanguage("HILFE"),
    };
}