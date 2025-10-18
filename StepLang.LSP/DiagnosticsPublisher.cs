using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using StepLang.Diagnostics;
using StepLang.Tooling.Diagnostics;
using StepDiagnostic = StepLang.Diagnostics.Diagnostic;
using OmniDiagnostic = OmniSharp.Extensions.LanguageServer.Protocol.Models.Diagnostic;
using OmniRange = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace StepLang.LSP;

internal sealed class DiagnosticsPublisher(
	DiagnosticsSessionState state,
	ILanguageServer server,
	ILogger<DiagnosticsPublisher> logger
)
{
	public void Register()
	{
		state.DiagnosticsPublished += OnDiagnosticsPublished;
	}

	private void OnDiagnosticsPublished(object? sender, DiagnosticsPublishedEventArgs e)
	{
		logger.LogTrace("Publishing diagnostics {@PublishedEventArgs}", e);

		var publishParams = new PublishDiagnosticsParams
		{
			Uri = e.DocumentUri,
			Version = e.Version,
			Diagnostics = Container.From(e.Diagnostics.Select(TranslateDiagnostic)),
		};

		server.PublishDiagnostics(publishParams);
	}

	private static OmniDiagnostic TranslateDiagnostic(StepDiagnostic d)
	{
		return new()
		{
			Code = new DiagnosticCode(d.Code),
			Message = d.Message,
			Range = d.Location?.ToRange() ?? new OmniRange(),
			Severity = d.Severity switch
			{
				Severity.Error => DiagnosticSeverity.Error,
				Severity.Warning => DiagnosticSeverity.Warning,
				Severity.Suggestion => DiagnosticSeverity.Information,
				Severity.Hint => DiagnosticSeverity.Hint,
				_ => throw new NotImplementedException("Severity is not implemented: " + d.Severity),
			},
		};
	}
}
