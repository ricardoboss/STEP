using StepLang.Parsing.Nodes;
using StepLang.Parsing.Nodes.VariableDeclarations;
using StepLang.Tokenizing;

namespace StepLang.Tooling.Diagnostics;

public class UsagesAnalyzer
{
	private readonly VariableDeclarationCollector collector;

	public UsagesAnalyzer(Uri documentUri, RootNode document)
	{
		collector = new VariableDeclarationCollector(documentUri);

		collector.Visit(document);
	}

	public IVariableDeclarationNode? FindDeclaration(Token identifier)
	{
		if (identifier.Type != TokenType.Identifier)
			throw new InvalidOperationException($"Expected identifier, got {identifier.Type}");

		// find the scope that contains the identifier
		var allScopes = collector.Scopes;
		var tightestScope = collector.RootScope;
		foreach (var scope in allScopes.OrderBy(s => s.OpenLocation.Line).ThenBy(s => s.OpenLocation.Column))
		{
			if (scope.OpenLocation.Line <= identifier.Location.Line &&
			    identifier.Location.Line <= scope.CloseLocation?.Line &&
			    scope.OpenLocation.Column <= identifier.Location.Column &&
			    identifier.Location.Column <= scope.CloseLocation?.Column)
			{
				tightestScope = scope;
			}
		}

		return tightestScope.FindDeclaration(identifier.Value);
	}
}
