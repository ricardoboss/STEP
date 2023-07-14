using System.Runtime.CompilerServices;
using HILFE.Statements;

namespace HILFE;

public static class Parser
{
    private enum State
    {
        LineStart,
        DeclarationType,
        DeclarationName,
        InitializerStart,
        InitializerIdentifier,
        InitializerLiteralString,
        InitializerLiteralNumber,
        IfStatement,
        IfExpression,
        IfExpressionEnd,
        IdentifierStart,
        IdentifierFunctionArgument,
        IdentifierAssignment,
        IdentifierFunctionArgumentEnd,
        IdentifierFunctionCallEnd,
        WhileStatement,
    }

    private static readonly Dictionary<State, Dictionary<(State, StatementType?), TokenType[]>> Transitions = new()
    {
        {
            State.LineStart,
            new()
            {
                { (State.LineStart, null), new[] { TokenType.Whitespace } },
                { (State.LineStart, StatementType.EmptyLine), new[] { TokenType.NewLine } },
                { (State.DeclarationType, null), new[] { TokenType.TypeName } },
                { (State.IfStatement, null), new [] { TokenType.IfKeyword } },
                { (State.IdentifierStart, null), new [] { TokenType.Identifier } },
                { (State.WhileStatement, null), new [] { TokenType.WhileKeyword } },
                { (State.LineStart, StatementType.CodeBlockStart), new []{ TokenType.CodeBlockOpener } },
                { (State.LineStart, StatementType.CodeBlockEnd), new []{ TokenType.CodeBlockCloser } },
            }
        },
        {
            State.DeclarationType,
            new()
            {
                { (State.DeclarationType, null), new[] { TokenType.Whitespace } },
                { (State.DeclarationName, null), new[] { TokenType.Identifier } },
            }
        },
        {
            State.DeclarationName,
            new()
            {
                { (State.DeclarationName, null), new[] { TokenType.Whitespace } },
                { (State.InitializerStart, null), new[] { TokenType.EqualsSymbol } },
                { (State.LineStart, StatementType.VariableDeclaration), new[] { TokenType.NewLine } },
            }
        },
        {
            State.InitializerStart,
            new()
            {
                { (State.InitializerStart, null), new[] { TokenType.Whitespace } },
                { (State.InitializerIdentifier, null), new[] { TokenType.Identifier } },
                { (State.InitializerLiteralString, null), new[] { TokenType.LiteralString } },
                { (State.InitializerLiteralNumber, null), new[] { TokenType.LiteralNumber } },
            }
        },
        {
            State.InitializerIdentifier,
            new()
            {
                { (State.LineStart, StatementType.VariableDeclaration), new[] { TokenType.NewLine } },
            }
        },
        {
            State.InitializerLiteralString,
            new()
            {
                { (State.InitializerLiteralString, null), new[] { TokenType.Whitespace } },
                { (State.LineStart, StatementType.VariableDeclaration), new[] { TokenType.NewLine } },
            }
        },
        {
            State.InitializerLiteralNumber,
            new()
            {
                { (State.LineStart, StatementType.VariableDeclaration), new[] { TokenType.NewLine } },
            }
        },
        {
            State.IfStatement,
            new()
            {
                { (State.IfStatement, null), new [] { TokenType.Whitespace } },
                { (State.IfExpression, null), new [] { TokenType.ExpressionOpener } },
            }
        },
        {
            State.IfExpression,
            new()
            {
                { (State.IfExpression, null), new [] { TokenType.Whitespace, TokenType.NewLine, TokenType.Identifier, TokenType.EqualsSymbol, TokenType.LiteralString, TokenType.LiteralNumber } },
                { (State.IfExpressionEnd, null), new [] { TokenType.ExpressionCloser } },
            }
        },
        {
            State.IfExpressionEnd,
            new()
            {
                { (State.IfExpressionEnd, null), new [] { TokenType.Whitespace, TokenType.NewLine } },
                { (State.LineStart, StatementType.IfStatement), new [] { TokenType.CodeBlockOpener } },
            }
        },
        {
            State.IdentifierStart,
            new()
            {
                { (State.IdentifierStart, null), new [] { TokenType.Whitespace } },
                { (State.IdentifierFunctionArgument, null), new [] { TokenType.ExpressionOpener } },
                { (State.IdentifierAssignment, null), new [] { TokenType.EqualsSymbol } },
            }
        },
        {
            State.IdentifierFunctionArgument,
            new()
            {
                { (State.IdentifierFunctionArgument, null), new [] { TokenType.Whitespace } },
                { (State.IdentifierFunctionArgumentEnd, null), new [] { TokenType.LiteralNumber, TokenType.LiteralString, TokenType.Identifier, } },
            }
        },
        {
            State.IdentifierFunctionArgumentEnd,
            new()
            {
                { (State.IdentifierFunctionArgumentEnd, null), new [] { TokenType.Whitespace } },
                { (State.IdentifierFunctionArgument, null), new [] { TokenType.ExpressionSeparator } },
                { (State.IdentifierFunctionCallEnd, null), new [] { TokenType.ExpressionCloser } },
            }
        },
        {
            State.IdentifierFunctionCallEnd,
            new()
            {
                { (State.LineStart, StatementType.FunctionCall), new [] { TokenType.NewLine } },
            }
        },
        {
            State.IdentifierAssignment,
            new()
            {
                { (State.IdentifierAssignment, null), new [] { TokenType.Whitespace } },
            }
        },
        {
            State.WhileStatement,
            new()
            {
                { (State.WhileStatement, null), new [] { TokenType.Whitespace } },
            }
        }
    };

    public static async IAsyncEnumerable<BaseStatement> ParseAsync(IAsyncEnumerable<Token> tokens, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var state = State.LineStart;
        var currentStatement = new List<Token>();

        await foreach (var token in tokens.WithCancellation(cancellationToken))
        {
            var stateTransitions = Transitions[state];
            (State, StatementType?)? acceptingTransition = null;
            List<TokenType> expectedTokenTypes = new();
            foreach (var transition in stateTransitions)
            {
                expectedTokenTypes.AddRange(transition.Value);
                if (!transition.Value.Contains(token.Type))
                    continue;

                acceptingTransition = transition.Key;
                break;
            }

            if (!acceptingTransition.HasValue)
            {
                #if DEBUG
                Console.WriteLine($"Processing token {token} in state {state}");
                #endif

                throw new UnexpectedTokenException(expectedTokenTypes, token);
            }

            var (nextState, statementType) = acceptingTransition.Value;

            state = nextState;

            currentStatement.Add(token);
            if (!statementType.HasValue)
                continue;

            var currentStatementTokens = currentStatement.ToArray();
            yield return statementType.Value switch
            {
                StatementType.VariableDeclaration => new VariableDeclarationStatement(currentStatementTokens),
                StatementType.EmptyLine => new EmptyLineStatement(currentStatementTokens),
                StatementType.IfStatement => new IfStatement(currentStatementTokens),
                StatementType.FunctionCall => new FunctionCallStatement(currentStatementTokens),
                StatementType.ElseStatement => new ElseStatement(currentStatementTokens),
                StatementType.CodeBlockStart => new CodeBlockStartStatement(currentStatementTokens),
                StatementType.CodeBlockEnd => new CodeBlockEndStatement(currentStatementTokens),
                _ => throw new NotImplementedException("Unimplemented StatementType: " + statementType.Value),
            };

            currentStatement.Clear();
        }

        if (currentStatement.Any(t => t.Type is not TokenType.Whitespace and not TokenType.NewLine))
            throw new InvalidOperationException("Unexpected end of input");
    }
}