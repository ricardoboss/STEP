using Microsoft.Extensions.Logging;
using StepLang.Diagnostics;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Parsing.Nodes;
using StepLang.Tokenizing;
using StepLang.Tooling.Diagnostics.Analyzers;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace StepLang.Tooling.Diagnostics;

public sealed class DiagnosticsSessionState
{
	private readonly ConcurrentDictionary<Uri, DocumentState> documents = new();
	public IReadOnlyDictionary<Uri, DocumentState> Documents => documents;

	private readonly ConcurrentDictionary<string, List<DeclaredVariableInfo>> symbols = new();
	public IReadOnlyDictionary<string, IReadOnlyList<DeclaredVariableInfo>> Symbols => symbols.ToImmutableDictionary(
		kvp => kvp.Key, IReadOnlyList<DeclaredVariableInfo> (kvp) => kvp.Value
	);

	public ConcurrentDictionary<Uri, ObservableCollection<Diagnostic>> Diagnostics { get; } = new();

	public event EventHandler<DiagnosticsPublishedEventArgs>? DiagnosticsPublished;

	public async Task AddDocumentAsync(DocumentState documentState, CancellationToken cancellationToken = default)
	{
		logger.LogDebug("Adding document {DocumentUri} to session", documentState.DocumentUri);

		if (!documents.TryAdd(documentState.DocumentUri, documentState))
		{
			throw new InvalidOperationException("Document already exists");
		}

		var collection = Diagnostics.AddOrUpdate(documentState.DocumentUri, [], (_, _) => []);
		collection.CollectionChanged += (_, _) => PublishDiagnostics(documentState.DocumentUri);

		await RecalculateDocumentAsync(documentState, cancellationToken);

		RecalculateSymbols();

		QueueAnalysis(documentState);
	}

	public async Task UpdateDocumentAsync(Uri documentUri, int version, Func<string, string> textUpdater,
		CancellationToken cancellationToken = default)
	{
		logger.LogDebug("Updating document {DocumentUri}", documentUri);

		if (!documents.Remove(documentUri, out var oldDocument))
		{
			logger.LogWarning("Document {DocumentUri} was not found in session; nothing to update", documentUri);

			return;
		}

		var collection = Diagnostics.AddOrUpdate(documentUri, [], (_, _) => []);
		collection.CollectionChanged += (_, _) => PublishDiagnostics(documentUri);

		var text = textUpdater(oldDocument.Text);

		var newDocumentState = new DocumentState
		{
			DocumentUri = documentUri, Version = version, Text = text,
		};

		documents[documentUri] = newDocumentState;

		await RecalculateDocumentAsync(newDocumentState, cancellationToken);

		RecalculateSymbols();

		QueueAnalysis(newDocumentState);
	}

	private void PublishDiagnostics(Uri documentUri)
	{
		logger.LogTrace("Sending diagnostics for document {DocumentUri}", documentUri);

		var args = new DiagnosticsPublishedEventArgs
		{
			DocumentUri = documentUri, Diagnostics = Diagnostics[documentUri], Version = Documents[documentUri].Version,
		};

		DiagnosticsPublished?.Invoke(this, args);
	}

	private async Task RecalculateDocumentAsync(DocumentState documentState, CancellationToken cancellationToken)
	{
		logger.LogTrace("Recalculating document {@Document}", documentState);

		try
		{
			var tokens = await TokenizeAsync(documentState, cancellationToken);
			documentState.Tokens = tokens;

			var ast = await ParseAstAsync(documentState, tokens, cancellationToken);
			documentState.Ast = ast;

			var declarations = await ScanDeclarationsAsync(documentState, ast, cancellationToken);
			documentState.Symbols = declarations;
		}
		catch (StepLangException e)
		{
			ReportStepLangException(documentState, e);
		}
	}

	private void ReportStepLangException(DocumentState documentState, StepLangException e)
	{
		logger.LogTrace("Reporting exception {ExceptionType} for document {@Document}", e.GetType().Name,
			documentState);

		var diagnostic = new Diagnostic
		{
			Code = e.ErrorCode, Message = e.Message, Severity = Severity.Error, Location = e.Location,
		};

		Diagnostics[documentState.DocumentUri].Add(diagnostic);
	}

	private readonly Lazy<IDiagnosticsReporter> lazyReporter;
	private readonly ILogger<DiagnosticsSessionState> logger;
	private readonly IDiagnosticsRunner diagnosticsRunner;

	public DiagnosticsSessionState(ILogger<DiagnosticsSessionState> logger, IDiagnosticsRunner diagnosticsRunner)
	{
		this.logger = logger;
		this.diagnosticsRunner = diagnosticsRunner;

		lazyReporter = new(() => new SessionStateDiagnosticsReporter(this));
	}

	private IDiagnosticsReporter Reporter => lazyReporter.Value;

	private void QueueAnalysis(DocumentState documentState)
	{
		logger.LogTrace("Queuing analysis for document {@Document}", documentState);

		_ = ThreadPool.QueueUserWorkItem(_ => diagnosticsRunner
			.RunDiagnosticsAsync(documentState, Reporter, CancellationToken.None)
			.ConfigureAwait(true)
			.GetAwaiter()
			.GetResult()
		);
	}

	private async Task<TokenCollection> TokenizeAsync(DocumentState documentState, CancellationToken cancellationToken)
	{
		logger.LogTrace("Tokenizing document {@Document}", documentState);

		var diagnostics = new DiagnosticCollection();
		var tokens = new Tokenizer(documentState.Text, diagnostics).Tokenize(cancellationToken).ToArray();

		foreach (var diagnostic in diagnostics)
			await Reporter.ReportAsync(documentState, diagnostic, cancellationToken);

		return new TokenCollection(tokens);
	}

	private async Task<RootNode> ParseAstAsync(DocumentState documentState, TokenCollection tokens,
		CancellationToken cancellationToken)
	{
		logger.LogTrace("Parsing AST for document {@Document}", documentState);

		var diagnostics = new DiagnosticCollection();
		var parser = new Parser(tokens, diagnostics);

		foreach (var diagnostic in diagnostics)
			await Reporter.ReportAsync(documentState, diagnostic, cancellationToken);

		return parser.ParseRoot();
	}

	private async Task<FileSymbols> ScanDeclarationsAsync(DocumentState documentState, RootNode ast,
		CancellationToken cancellationToken)
	{
		logger.LogTrace("Scanning declarations for document {@Document}", documentState);

		var collector = new VariableDeclarationCollector(documentState.DocumentUri);

		collector.Visit(ast);

		await ReportUndefinedSymbolsAsync(documentState, collector, cancellationToken);

		return new FileSymbols
		{
			DocumentUri = documentState.DocumentUri, Declarations = collector.Declarations, Scopes = collector.Scopes,
		};
	}

	private async Task ReportUndefinedSymbolsAsync(DocumentState documentState, VariableDeclarationCollector collector,
		CancellationToken cancellationToken)
	{
		var scopes = new Queue<VariableScope>();
		scopes.Enqueue(collector.RootScope);

		while (scopes.TryDequeue(out var currentScope))
		{
			foreach (var (identifierName, usages) in currentScope.Usages)
			{
				var isBuiltIn = Scope.GlobalScope.Exists(identifierName, includeParent: false);
				if (isBuiltIn)
					continue;

				var isDefinedInCurrentScope = currentScope.FindDeclaration(identifierName) != null;
				if (isDefinedInCurrentScope)
					continue;

				foreach (var diagnostic in usages.Select(usage => new Diagnostic
				         {
					         Severity = Severity.Error,
					         Message = $"Use of undefined symbol: {identifierName}",
					         Code = "undefined-symbol",
					         Token = usage,
				         }))
				{
					await Reporter.ReportAsync(documentState, diagnostic, cancellationToken);
				}
			}

			foreach (var childScope in currentScope.Children)
				scopes.Enqueue(childScope);
		}
	}

	private void RecalculateSymbols()
	{
		logger.LogTrace("Recalculating symbols");

		HashSet<(string Name, Uri DocumentUri)> seen = [];
		foreach (var document in documents.Values)
		{
			var declarations = document.Symbols?.Declarations ?? [];
			foreach (var declaration in declarations)
			{
				_ = symbols.AddOrUpdate(declaration.Name, [declaration], (key, others) =>
				{
					// remove all declarations with the same name from the same document
					_ = others.RemoveAll(d => d.DocumentUri == document.DocumentUri);

					others.Add(declaration);

					return others;
				});

				_ = seen.Add((declaration.Name, document.DocumentUri));
			}
		}

		// delete all symbols that were not seen in the current documents
		foreach (var name in symbols.Keys.Except(seen.Select(t => t.Name)))
		{
			_ = symbols.TryRemove(name, out _);
		}
	}

	public void RemoveDocument(Uri documentUri)
	{
		logger.LogTrace("Removing document {DocumentUri} from session", documentUri);

		if (!documents.Remove(documentUri, out _))
		{
			logger.LogWarning("Document {DocumentUri} was not found in session; nothing to remove", documentUri);

			return;
		}

		_ = Diagnostics.Remove(documentUri, out _);

		RecalculateSymbols();
	}
}
