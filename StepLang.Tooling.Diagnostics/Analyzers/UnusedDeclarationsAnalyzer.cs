using Microsoft.Extensions.Logging;
using StepLang.Diagnostics;

namespace StepLang.Tooling.Diagnostics.Analyzers;

public sealed class UnusedDeclarationsAnalyzer(ILogger<UnusedDeclarationsAnalyzer> logger) : IAnalyzer
{
	private const string DiagnosticCode = "unused-declaration";

	public Task AnalyzeAsync(SessionState sessionState, DocumentState documentState,
		CancellationToken cancellationToken)
	{
		var symbols = documentState.Symbols;
		if (symbols is null)
		{
			logger.LogWarning("Document {Document} has no symbols; skipping unused declarations analysis", documentState);

			return Task.CompletedTask;
		}

		var declarations = symbols.Declarations;
		var unused = declarations
			.Select(declaration => new
			{
				declaration, anyUsage = symbols.Scopes.Any(scope => scope.Usages.ContainsKey(declaration.Name)),
			})
			.Where(t => !t.anyUsage)
			.Select(t => t.declaration)
			.ToList();

		if (unused.Count == 0)
		{
			logger.LogTrace("Document {Document} has no unused declarations", documentState);

			return Task.CompletedTask;
		}

		var diagnostics = unused.Select(CreateDiagnostic).ToList();
		var collection = sessionState.Diagnostics[documentState.DocumentUri];

		foreach (var diagnostic in diagnostics)
			collection.Add(diagnostic);

		return Task.CompletedTask;
	}

	private Diagnostic CreateDiagnostic(DeclaredVariableInfo unusedDeclaration)
	{
		logger.LogTrace("Creating diagnostic for unused declaration {Declaration}", unusedDeclaration);

		return new()
		{
			Code = DiagnosticCode,
			Message = $"Unused declaration '{unusedDeclaration.Name}'",
			Severity = Severity.Hint,
			Token = unusedDeclaration.Declaration.Identifier,
		};
	}
}
