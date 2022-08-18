using System.Runtime.CompilerServices;

namespace HILFE;

public static class Parser
{
    private enum State
    {
        S,
        D1,
        D2,
        I1,
        I2,
        St1,
        N1,
    }

    private static readonly Dictionary<State, Dictionary<(State, StatementType?), TokenType[]>> Transitions = new()
    {
        {
            State.S,
            new()
            {
                { (State.S, null), new[] { TokenType.Whitespace, TokenType.NewLine } },
                { (State.D1, null), new[] { TokenType.TypeName } },
            }
        },
        {
            State.D1,
            new()
            {
                { (State.D1, null), new[] { TokenType.Whitespace } },
                { (State.D2, null), new[] { TokenType.Identifier } },
            }
        },
        {
            State.D2,
            new()
            {
                { (State.D2, null), new[] { TokenType.Whitespace } },
                { (State.I1, null), new[] { TokenType.EqualsSymbol } },
                { (State.S, StatementType.VariableDeclaration), new[] { TokenType.NewLine } },
            }
        },
        {
            State.I1,
            new()
            {
                { (State.I1, null), new[] { TokenType.Whitespace } },
                { (State.I2, null), new[] { TokenType.Identifier } },
                { (State.St1, null), new[] { TokenType.LiteralString } },
                { (State.N1, null), new[] { TokenType.LiteralNumber } },
            }
        },
        {
            State.I2,
            new()
            {
                { (State.S, StatementType.VariableDeclaration), new[] { TokenType.NewLine } },
            }
        },
        {
            State.St1,
            new()
            {
                { (State.St1, null), new[] { TokenType.Whitespace } },
                { (State.S, StatementType.VariableDeclaration), new[] { TokenType.NewLine } },
            }
        },
        {
            State.N1,
            new()
            {
                { (State.S, StatementType.VariableDeclaration), new[] { TokenType.NewLine } },
            }
        },
    };

    public static async IAsyncEnumerable<Statement> ParseAsync(IAsyncEnumerable<Token> tokens, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var state = State.S;
        var currentStatement = new List<Token>();

        await foreach (var token in tokens.WithCancellation(cancellationToken))
        {
            var stateTransitions = Transitions[state];
            KeyValuePair<(State, StatementType?), TokenType[]>? acceptingTransition = null;
            List<TokenType> expectedTokenTypes = new();
            foreach (var transition in stateTransitions)
            {
                expectedTokenTypes.AddRange(transition.Value);
                if (!transition.Value.Contains(token.Type))
                    continue;

                acceptingTransition = transition;
                break;
            }

            if (!acceptingTransition.HasValue)
                throw new UnexpectedTokenException(expectedTokenTypes, token);

            var (nextState, statementType) = acceptingTransition.Value.Key;

            state = nextState;

            currentStatement.Add(token);
            if (!statementType.HasValue)
                continue;

            yield return new(statementType.Value, currentStatement.ToArray());

            currentStatement.Clear();
        }

        if (currentStatement.Any(t => t.Type is not TokenType.Whitespace and not TokenType.NewLine))
            throw new InvalidOperationException("Unexpected end of input");
    }
}