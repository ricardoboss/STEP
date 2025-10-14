using Microsoft.Extensions.Logging;
using StepLang.Diagnostics;

namespace StepLang.Tooling.Diagnostics.Analyzers;

public sealed class UnusedDeclarationsDiagnosticsAnalyzer(ILogger<UnusedDeclarationsDiagnosticsAnalyzer> logger)
	: IDiagnosticsAnalyzer
{
	private const string DiagnosticCode = "unused-declaration";

	public async Task AnalyzeAsync(DocumentState state, IDiagnosticsReporter reporter,
		CancellationToken cancellationToken = default)
	{
		var symbols = state.Symbols;
		if (symbols is null)
		{
			logger.LogWarning("Document {@Document} has no symbols; skipping unused declarations analysis", state);

			return;
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
			logger.LogTrace("Document {@Document} has {TotalDeclarations} declarations and no unused ones",
				state, declarations.Count);

			return;
		}

		logger.LogTrace("Document {@Document} has {TotalDeclarations} declarations", state, declarations.Count);

		foreach (var diagnostic in unused.Select(CreateDiagnostic))
			await reporter.ReportAsync(state, diagnostic, cancellationToken);
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
