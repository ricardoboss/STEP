using MediatR;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;

namespace StepLang.LSP;

public class TextDocumentHandler : TextDocumentSyncHandlerBase
{
    private readonly ILogger<TextDocumentHandler> _logger;
    private readonly ILanguageServerConfiguration _configuration;

    private readonly TextDocumentSelector _documentSelector = TextDocumentSelector.ForLanguage("STEP");

    /// <inheritdoc />
    public TextDocumentHandler(ILogger<TextDocumentHandler> logger, ILanguageServerConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public TextDocumentSyncKind Change { get; } = TextDocumentSyncKind.Full;

    public override Task<Unit> Handle(DidChangeTextDocumentParams notification, CancellationToken token)
    {
        // _logger.LogCritical("Critical");
        _logger.LogDebug("Debug");
        _logger.LogTrace("Trace");
        _logger.LogInformation("Hello world!");
        return Unit.Task;
    }

    public override async Task<Unit> Handle(DidOpenTextDocumentParams notification, CancellationToken token)
    {
        await Task.Yield();
        _logger.LogInformation("Hello world!");
        await _configuration.GetScopedConfiguration(notification.TextDocument.Uri, token).ConfigureAwait(false);
        return Unit.Value;
    }

    public override Task<Unit> Handle(DidCloseTextDocumentParams notification, CancellationToken token)
    {
        if (_configuration.TryGetScopedConfiguration(notification.TextDocument.Uri, out var disposable))
        {
            disposable.Dispose();
        }

        return Unit.Task;
    }

    public override Task<Unit> Handle(DidSaveTextDocumentParams notification, CancellationToken token) => Unit.Task;

    protected override TextDocumentSyncRegistrationOptions CreateRegistrationOptions(TextSynchronizationCapability? capability, ClientCapabilities? clientCapabilities)
    {
        return new()
        {
            DocumentSelector = _documentSelector,
            Change = Change,
            Save = new SaveOptions
            {
                IncludeText = true,
            },
        };
    }

    public override TextDocumentAttributes GetTextDocumentAttributes(DocumentUri uri) => new(uri, "STEP");
}