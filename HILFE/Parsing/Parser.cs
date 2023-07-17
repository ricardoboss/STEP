using System.Runtime.CompilerServices;
using HILFE.Parsing.Statements;
using HILFE.Tokenizing;

namespace HILFE.Parsing;

public class Parser
{
    public enum State
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
        ElseStatement,
        IfElseStatement,
        IfElseExpression,
        IdentifierStart,
        IdentifierFunctionArgument,
        IdentifierAssignment,
        IdentifierFunctionArgumentEnd,
        WhileStatement,
        WhileExpression,
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
                { (State.ElseStatement, null), new [] { TokenType.ElseKeyword } },
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
                { (State.LineStart, StatementType.IfStatement), new [] { TokenType.ExpressionCloser } },
            }
        },
        {
            State.ElseStatement,
            new()
            {
                { (State.LineStart, StatementType.ElseStatement), new [] { TokenType.Whitespace, TokenType.NewLine } },
                { (State.IfElseStatement, null), new [] { TokenType.IfKeyword } },
            }
        },
        {
            State.IfElseStatement,
            new()
            {
                { (State.IfElseStatement, null), new [] { TokenType.Whitespace, TokenType.NewLine } },
                { (State.IfElseExpression, null), new [] { TokenType.ExpressionOpener } },
            }
        },
        {
            State.IfElseExpression,
            new()
            {
                { (State.IfElseExpression, null), new [] { TokenType.Whitespace, TokenType.NewLine, TokenType.Identifier, TokenType.EqualsSymbol, TokenType.LiteralString, TokenType.LiteralNumber } },
                { (State.LineStart, StatementType.IfElseStatement), new [] { TokenType.ExpressionCloser } },
            }
        },
        {
            State.IdentifierStart,
            new()
            {
                { (State.IdentifierStart, null), new [] { TokenType.Whitespace, TokenType.NewLine } },
                { (State.IdentifierFunctionArgument, null), new [] { TokenType.ExpressionOpener } },
                { (State.IdentifierAssignment, null), new [] { TokenType.EqualsSymbol } },
            }
        },
        {
            State.IdentifierFunctionArgument,
            new()
            {
                { (State.IdentifierFunctionArgument, null), new [] { TokenType.Whitespace, TokenType.NewLine } },
                { (State.IdentifierFunctionArgumentEnd, null), new [] { TokenType.LiteralNumber, TokenType.LiteralString, TokenType.LiteralBoolean, TokenType.Identifier } },
                { (State.LineStart, StatementType.FunctionCall), new [] { TokenType.ExpressionCloser } },
            }
        },
        {
            State.IdentifierFunctionArgumentEnd,
            new()
            {
                { (State.IdentifierFunctionArgumentEnd, null), new [] { TokenType.Whitespace, TokenType.NewLine } },
                { (State.IdentifierFunctionArgument, null), new [] { TokenType.ExpressionSeparator } },
                { (State.LineStart, StatementType.FunctionCall), new [] { TokenType.ExpressionCloser } },
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
                { (State.WhileStatement, null), new [] { TokenType.Whitespace, TokenType.NewLine } },
                { (State.WhileExpression, null), new [] { TokenType.ExpressionOpener } },
            }
        },
        {
            State.WhileExpression,
            new()
            {
                { (State.WhileExpression, null), new [] { TokenType.Whitespace, TokenType.NewLine, TokenType.Identifier, TokenType.EqualsSymbol, TokenType.LiteralString, TokenType.LiteralNumber } },
                { (State.LineStart, StatementType.WhileStatement), new [] { TokenType.ExpressionCloser } },
            }
        },
    };

    private readonly List<Token> currentStatement = new();
    private State state;
    private int codeBlockDepth;

    public Parser(State state = State.LineStart)
    {
        this.state = state;
    }

    public IAsyncEnumerable<BaseStatement> ParseAsync(IEnumerable<Token> tokens, CancellationToken cancellationToken = default)
    {
        return ParseAsync(tokens.ToAsyncEnumerable(), cancellationToken);
    }

    public async IAsyncEnumerable<BaseStatement> ParseAsync(IAsyncEnumerable<Token> tokens, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var token in tokens.WithCancellation(cancellationToken))
        {
            var (nextState, statementType) = CalculateAcceptingTransition(token);

            currentStatement.Add(token);

            if (statementType.HasValue)
                yield return BuildStatement(statementType.Value);

            state = nextState;
        }
    }

    private (State, StatementType?) CalculateAcceptingTransition(Token token)
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
            throw new UnexpectedTokenException(state, expectedTokenTypes, token);

        return acceptingTransition.Value;
    }

    private BaseStatement BuildStatement(StatementType type)
    {
        var currentStatementTokens = currentStatement.ToArray();
        currentStatement.Clear();

        BaseStatement statement = type switch
        {
            StatementType.VariableDeclaration => new VariableDeclarationStatement(currentStatementTokens),
            StatementType.EmptyLine => new EmptyLineStatement(currentStatementTokens),
            StatementType.IfStatement => new IfStatement(currentStatementTokens),
            StatementType.ElseStatement => new ElseStatement(currentStatementTokens),
            StatementType.IfElseStatement => new IfElseStatement(currentStatementTokens),
            StatementType.WhileStatement => new WhileStatement(currentStatementTokens),
            StatementType.FunctionCall => new FunctionCallStatement(currentStatementTokens),
            StatementType.CodeBlockStart => new CodeBlockStartStatement(currentStatementTokens),
            StatementType.CodeBlockEnd => new CodeBlockEndStatement(currentStatementTokens),
            _ => throw new NotImplementedException("Unimplemented StatementType: " + type),
        };

        switch (statement)
        {
            case CodeBlockStartStatement:
                codeBlockDepth++;
                break;
            case CodeBlockEndStatement:
                codeBlockDepth--;
                break;
        }

        return statement;
    }

    public void End()
    {
        if (currentStatement.Any(t => t.Type is not TokenType.Whitespace and not TokenType.NewLine))
            throw new UnexpectedEndOfInputException(state, "Unexpected end of input: expected whitespace or newline");

        if (codeBlockDepth != 0)
            throw new ImbalancedCodeBlocksException(state, "Unexpected end of input: imbalanced code blocks");
    }

    ~Parser() => End();
}