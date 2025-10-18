using MediatR;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using StepLang.Tooling.Diagnostics;

namespace StepLang.LSP.Handlers.TextDocument;

internal sealed class DidOpenTextDocumentHandler(ILogger<DidOpenTextDocumentHandler> logger, DiagnosticsSessionState state)
	: DidOpenTextDocumentHandlerBase
{
	protected override TextDocumentOpenRegistrationOptions CreateRegistrationOptions(
		TextSynchronizationCapability capability, ClientCapabilities clientCapabilities)
	{
		return new TextDocumentOpenRegistrationOptions
		{
			DocumentSelector = StepTextDocumentSelector.Instance,
		};
	}

	public override async Task<Unit> Handle(DidOpenTextDocumentParams request, CancellationToken cancellationToken)
	{
		logger.LogTrace("Handling DidOpenTextDocument");

		await AddDocumentToSessionAsync(request.TextDocument, cancellationToken);

		return Unit.Value;
	}

	private async Task AddDocumentToSessionAsync(TextDocumentItem document, CancellationToken cancellationToken)
	{
		var documentState = new DocumentState
		{
			DocumentUri = document.Uri.ToUri(), Version = document.Version ?? 0, Text = document.Text,
		};

		await state.AddDocumentAsync(documentState, cancellationToken);
	}
}
