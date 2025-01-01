using StepLang.Diagnostics;
using StepLang.Parsing.Nodes.Statements;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public static class ParsingDiagnosticCollectionExtensions
{
	public static ErrorStatementNode AddUnexpectedToken(this DiagnosticCollection collection, Token errorToken,
		params TokenType[] allowed)
	{
		var typeInfo = allowed.Length switch
		{
			0 => "any token",
			1 => $"a {allowed[0].ToDisplay()}",
			_ => $"any one of {string.Join(", ", allowed.Select(TokenTypes.ToDisplay))}",
		};

		collection.Add(DiagnosticArea.Parsing, Severity.Error, $"Unexpected {errorToken.Type.ToDisplay()}, expected {typeInfo}", "PAR001", errorToken);

		return new ErrorStatementNode($"Unexpected {errorToken.Type.ToDisplay()}, expected {typeInfo}", [errorToken]);
	}

	public static ErrorStatementNode AddUnexpectedEndOfTokens(this DiagnosticCollection collection, Token? lastToken)
	{
		collection.Add(DiagnosticArea.Parsing, Severity.Error, "Unexpected end of tokens", "PAR004", lastToken);

		return new ErrorStatementNode("Unexpected end of tokens", [lastToken]);
	}
}
