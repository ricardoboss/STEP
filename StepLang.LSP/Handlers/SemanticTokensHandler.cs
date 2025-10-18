using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using StepLang.Tokenizing;
using StepLang.Tooling.Diagnostics;

namespace StepLang.LSP.Handlers;

internal sealed class SemanticTokensHandler(ILogger<SemanticTokensHandler> logger, DiagnosticsSessionState state)
	: SemanticTokensHandlerBase
{
	protected override SemanticTokensRegistrationOptions CreateRegistrationOptions(SemanticTokensCapability? capability,
		ClientCapabilities clientCapabilities)
	{
		logger.LogTrace($"Creating registration options for {nameof(SemanticTokensHandler)}");

		var legend = new SemanticTokensLegend
		{
			TokenModifiers = Container.From(Array.Empty<SemanticTokenModifier>()),
			TokenTypes = Container.From(Enum.GetValues<TokenType>().Select(TranslateTokenType).Distinct()),
		};

		return new SemanticTokensRegistrationOptions
		{
			DocumentSelector = StepTextDocumentSelector.Instance,
			Legend = legend,
			Full = true,
			Range = true,
		};
	}

	protected override Task Tokenize(
		SemanticTokensBuilder builder,
		ITextDocumentIdentifierParams identifier,
		CancellationToken cancellationToken
	)
	{
		var document = state.Documents[identifier.TextDocument.Uri.ToUri()];
		if (document.Tokens is not { } tokens)
		{
			logger.LogWarning("Document {Document} is not fully processed; skipping semantic tokens", document);

			return Task.CompletedTask;
		}

		foreach (var token in tokens.Where(t =>
			         t.Type is not TokenType.Whitespace and not TokenType.NewLine and not TokenType.EndOfFile))
		{
			// shift by 1 because tokens in STEP are 1-indexed
			builder.Push(
				token.Location.Line - 1,
				token.Location.Column - 1,
				token.Location.Length,
				TranslateTokenType(token.Type),
				Array.Empty<SemanticTokenModifier>()
			);
		}

		return Task.CompletedTask;
	}

	protected override async Task<SemanticTokensDocument> GetSemanticTokensDocument(
		ITextDocumentIdentifierParams @params, CancellationToken cancellationToken)
	{
		var document = new SemanticTokensDocument(RegistrationOptions.Legend);
		var builder = new SemanticTokensBuilder(document, RegistrationOptions.Legend);

		await Tokenize(builder, @params, cancellationToken);

		return builder.Commit();
	}

	private static SemanticTokenType TranslateTokenType(TokenType type)
	{
		return type switch
		{
			TokenType.TypeName => "type",
			TokenType.Identifier => "variable",
			TokenType.IfKeyword => "keyword",
			TokenType.ElseKeyword => "keyword",
			TokenType.WhileKeyword => "keyword",
			TokenType.BreakKeyword => "keyword",
			TokenType.ContinueKeyword => "keyword",
			TokenType.LineComment => "comment",
			TokenType.ImportKeyword => "keyword",
			TokenType.ReturnKeyword => "keyword",
			TokenType.ForEachKeyword => "keyword",
			TokenType.InKeyword => "keyword",
			TokenType.LiteralNumber => "number",
			TokenType.LiteralString => "string",
			TokenType.EqualsSymbol => "operator",
			TokenType.GreaterThanSymbol => "operator",
			TokenType.LessThanSymbol => "operator",
			TokenType.PlusSymbol => "operator",
			TokenType.MinusSymbol => "operator",
			TokenType.AsteriskSymbol => "operator",
			TokenType.SlashSymbol => "operator",
			TokenType.PercentSymbol => "operator",
			TokenType.PipeSymbol => "operator",
			TokenType.AmpersandSymbol => "operator",
			TokenType.ExclamationMarkSymbol => "operator",
			TokenType.HatSymbol => "operator",
			TokenType.QuestionMarkSymbol => "operator",
			_ => type.ToString(),
		};
	}
}
