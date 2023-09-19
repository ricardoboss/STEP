using System.Diagnostics;
using System.Runtime.CompilerServices;
using StepLang.Expressions;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Statements;

public class StatementParser
{
    private readonly TokenQueue tokenQueue = new();

    private bool importsProhibited;

    public void Add(Token token) => tokenQueue.Enqueue(token);

    public void Add(IEnumerable<Token> tokens) => tokenQueue.Enqueue(tokens);

    public async Task AddAsync(IAsyncEnumerable<Token> tokens, CancellationToken cancellationToken = default)
    {
        await foreach (var token in tokens.WithCancellation(cancellationToken))
            tokenQueue.Enqueue(token);
    }

    public async IAsyncEnumerable<Statement> ParseAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (tokenQueue.TryDequeue(out var token))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var type = token.Type;

            switch (type)
            {
                case TokenType.Whitespace or TokenType.NewLine or TokenType.LineComment:
                    continue;
                case TokenType.ImportKeyword when importsProhibited:
                    throw new ImportsNoLongerAllowedException(token);
                case TokenType.ImportKeyword:
                    yield return await ParseImportStatement(token, cancellationToken);

                    continue;
            }

            if (!importsProhibited)
                importsProhibited = true;

            switch (type)
            {
                case TokenType.TypeName:
                    yield return await ParseVariableDeclaration(token, cancellationToken);
                    continue;
                case TokenType.Identifier or TokenType.UnderscoreSymbol:
                    yield return await ParseIdentifierUsage(token, cancellationToken);
                    continue;
                case TokenType.IfKeyword:
                    yield return await ParseIfStatement(token, cancellationToken);
                    continue;
                case TokenType.WhileKeyword:
                    yield return await ParseWhileLoop(token, cancellationToken);
                    continue;
                case TokenType.ReturnKeyword:
                    yield return await ParseReturnStatement(token, cancellationToken);
                    continue;
                case TokenType.BreakKeyword:
                    yield return await ParseBreakStatement(token, cancellationToken);
                    continue;
                case TokenType.ContinueKeyword:
                    yield return await ParseContinueStatement(token, cancellationToken);
                    continue;
                case TokenType.OpeningCurlyBracket:
                    yield return await ParseAnonymousCodeBlock(token, cancellationToken);
                    continue;
                case TokenType.ClosingCurlyBracket:
                    yield break;
            }

            var allowed = new[]
            {
                TokenType.TypeName, TokenType.Identifier,
                TokenType.UnderscoreSymbol, TokenType.Whitespace,
                TokenType.NewLine, TokenType.LineComment,
                TokenType.IfKeyword, TokenType.WhileKeyword,
                TokenType.ReturnKeyword, TokenType.BreakKeyword,
                TokenType.OpeningCurlyBracket, TokenType.ClosingCurlyBracket,
            };

            if (!importsProhibited)
                allowed = allowed.Append(TokenType.ImportKeyword).ToArray();

            throw new UnexpectedTokenException(token, allowed);
        }
    }

    private Task<Statement> ParseImportStatement(Token importToken, CancellationToken cancellationToken = default)
    {
        // import: import <literal string>

        cancellationToken.ThrowIfCancellationRequested();

        var literalStringToken = tokenQueue.Dequeue(TokenType.LiteralString);

        return Task.FromResult<Statement>(new ImportStatement(importToken, literalStringToken));
    }

    private async Task<Statement> ParseContinueStatement(Token continueToken, CancellationToken cancellationToken = default)
    {
        // continue: continue [expression]

        var expressionTokens = tokenQueue.DequeueUntil(TokenType.NewLine);

        if (tokenQueue.IsNotEmpty)
            tokenQueue.Dequeue(TokenType.NewLine);

        Expression expression;
        if (expressionTokens.Count == 0)
            expression = LiteralExpression.Number(1);
        else
            expression = await ExpressionParser.ParseAsync(expressionTokens, cancellationToken);

        return new ContinueStatement(continueToken, expression);
    }

    private async Task<Statement> ParseBreakStatement(Token breakToken, CancellationToken cancellationToken = default)
    {
        // break: break [expression]

        var expressionTokens = tokenQueue.DequeueUntil(TokenType.NewLine);

        if (tokenQueue.IsNotEmpty)
            tokenQueue.Dequeue(TokenType.NewLine);

        Expression expression;
        if (expressionTokens.Count == 0)
            expression = LiteralExpression.Number(1);
        else
            expression = await ExpressionParser.ParseAsync(expressionTokens, cancellationToken);

        return new BreakStatement(breakToken, expression);
    }

    private async Task<Statement> ParseReturnStatement(Token returnToken, CancellationToken cancellationToken = default)
    {
        // return: return <expression>

        var expressionTokens = tokenQueue.DequeueUntil(TokenType.NewLine);

        tokenQueue.Dequeue(TokenType.NewLine);

        var expression = await ExpressionParser.ParseAsync(expressionTokens, cancellationToken);

        return new ReturnStatement(expression)
        {
            Location = returnToken.Location,
        };
    }

    private async Task<Statement> ParseAnonymousCodeBlock(Token openingToken, CancellationToken cancellationToken)
    {
        // anonymous code block: { [statement]* }

        var statementsTokens = tokenQueue.DequeueUntil(TokenType.ClosingCurlyBracket);
        tokenQueue.Dequeue(TokenType.ClosingCurlyBracket);

        var statements = await ParseStatements(statementsTokens, cancellationToken).ToListAsync(cancellationToken);

        return new AnonymousCodeBlockStatement(statements)
        {
            Location = openingToken.Location,
        };
    }

    private async Task<Statement> ParseWhileLoop(Token whileToken, CancellationToken cancellationToken)
    {
        // looping: while (<expression>) { [statement]* }

        tokenQueue.Dequeue(TokenType.OpeningParentheses);
        var expressionTokens = tokenQueue.DequeueUntil(TokenType.ClosingParentheses);
        tokenQueue.Dequeue(TokenType.ClosingParentheses);

        tokenQueue.Dequeue(TokenType.OpeningCurlyBracket);
        var statementsTokens = tokenQueue.DequeueUntil(TokenType.ClosingCurlyBracket);
        tokenQueue.Dequeue(TokenType.ClosingCurlyBracket);

        var condition = await ExpressionParser.ParseAsync(expressionTokens, cancellationToken);
        var statements = await ParseStatements(statementsTokens, cancellationToken).ToListAsync(cancellationToken);

        return new WhileStatement(condition, statements)
        {
            Location = whileToken.Location,
        };
    }

    private async Task<Statement> ParseIfStatement(Token ifToken, CancellationToken cancellationToken)
    {
        // branching: if (<expression>) { [statement]* } [else[if (<expression>)] { [statement]* } ]

        tokenQueue.Dequeue(TokenType.OpeningParentheses);
        var expressionTokens = tokenQueue.DequeueUntil(TokenType.ClosingParentheses);
        tokenQueue.Dequeue(TokenType.ClosingParentheses);

        tokenQueue.Dequeue(TokenType.OpeningCurlyBracket);
        var trueBranchTokens = tokenQueue.DequeueUntil(TokenType.ClosingCurlyBracket);
        tokenQueue.Dequeue(TokenType.ClosingCurlyBracket);

        Expression condition;
        try
        {
            condition = await ExpressionParser.ParseAsync(expressionTokens, cancellationToken);
        }
        catch (UnexpectedEndOfTokensException e)
        {
            throw new MissingConditionExpressionException(ifToken, e);
        }

        var statements = await ParseStatements(trueBranchTokens, cancellationToken).ToListAsync(cancellationToken);

        if (tokenQueue.PeekType() is not TokenType.ElseKeyword)
            return new IfStatement(condition, statements)
            {
                Location = ifToken.Location,
            };

        tokenQueue.Dequeue(TokenType.ElseKeyword);

        IReadOnlyList<Token>? elseExpressionTokens = null;
        if (tokenQueue.PeekType() is TokenType.IfKeyword)
        {
            tokenQueue.Dequeue(TokenType.OpeningParentheses);
            elseExpressionTokens = tokenQueue.DequeueUntil(TokenType.ClosingParentheses);
            tokenQueue.Dequeue(TokenType.ClosingParentheses);
        }

        tokenQueue.Dequeue(TokenType.OpeningCurlyBracket);
        var falseBranchTokens = tokenQueue.DequeueUntil(TokenType.ClosingCurlyBracket);
        tokenQueue.Dequeue(TokenType.ClosingCurlyBracket);

        var elseExpression =
            elseExpressionTokens is not null ? await ExpressionParser.ParseAsync(elseExpressionTokens, cancellationToken) : null;

        var elseStatements = await ParseStatements(falseBranchTokens, cancellationToken)
            .ToListAsync(cancellationToken);

        return new IfElseStatement(condition, statements, elseExpression, elseStatements)
        {
            Location = ifToken.Location,
        };
    }

    private async Task<Statement> ParseIdentifierUsage(Token identifierToken, CancellationToken cancellationToken = default)
    {
        var nextType = tokenQueue.PeekType();
        if (nextType is TokenType.OpeningParentheses)
            return await ParseFunctionCall(identifierToken, cancellationToken);

        if (nextType.IsMathematicalOperation())
            return await ParseMathematicalOperation(identifierToken, cancellationToken);

        return await ParseVariableAssignment(identifierToken, cancellationToken);
    }

    private async Task<Statement> ParseVariableAssignment(Token identifierToken, CancellationToken cancellationToken = default)
    {
        // variable assignment: <identifier> = <expression> / _ = <expression> / <list variable>[<expression>] = <expression>

        if (tokenQueue.PeekType() is TokenType.OpeningSquareBracket)
            return await ParseIndexAssignment(identifierToken, cancellationToken);

        var equalsToken = tokenQueue.Dequeue(TokenType.EqualsSymbol);

        Expression expression;
        try
        {
            expression = await ParseValueExpression(cancellationToken);
        }
        catch (UnexpectedEndOfTokensException e)
        {
            throw new MissingValueExpressionException(equalsToken, e);
        }

        if (identifierToken.Type is TokenType.UnderscoreSymbol)
            return new DiscardAssignmentStatement(expression);

        return new VariableAssignmentStatement(identifierToken, expression);
    }

    private async Task<Statement> ParseIndexAssignment(Token identifierToken, CancellationToken cancellationToken = default)
    {
        // index assignment: <list variable>[<expression>] = <expression>

        _ = tokenQueue.Dequeue(TokenType.OpeningSquareBracket);

        var indexExpressionTokens = tokenQueue.DequeueUntil(TokenType.ClosingSquareBracket);

        _ = tokenQueue.Dequeue(TokenType.ClosingSquareBracket);

        var equalsToken = tokenQueue.Dequeue(TokenType.EqualsSymbol);

        var indexExpression = await ExpressionParser.ParseAsync(indexExpressionTokens, cancellationToken);

        Expression valueExpression;
        try
        {
            valueExpression = await ParseValueExpression(cancellationToken);
        }
        catch (UnexpectedEndOfTokensException e)
        {
            throw new MissingValueExpressionException(equalsToken, e);
        }

        return new IndexAssignmentStatement(identifierToken, indexExpression, valueExpression)
        {
            Location = identifierToken.Location,
        };
    }

    private async Task<Expression> ParseValueExpression(CancellationToken cancellationToken)
    {
        // FIXME: this does not work with multi-line values like function definitions
        var expressionTokens = tokenQueue.DequeueUntil(TokenType.NewLine);

        if (tokenQueue.IsNotEmpty)
            _ = tokenQueue.Dequeue(TokenType.NewLine);

        return await ExpressionParser.ParseAsync(expressionTokens, cancellationToken);
    }

    private async Task<Statement> ParseMathematicalOperation(Token identifierToken, CancellationToken cancellationToken = default)
    {
        // shortcut setter: <identifier>++ / <identifier>-- / <identifier> *= <expression> / <identifier> /= <expression>

        var operationToken = tokenQueue.Dequeue(TokenType.PlusSymbol,
            TokenType.MinusSymbol, TokenType.AsteriskSymbol, TokenType.SlashSymbol,
            TokenType.PercentSymbol);

        var valueDenominator = tokenQueue.Dequeue(TokenType.PlusSymbol,
            TokenType.MinusSymbol, TokenType.AsteriskSymbol, TokenType.SlashSymbol,
            TokenType.PercentSymbol, TokenType.EqualsSymbol);

        IReadOnlyList<Token> valueExpTokens;
        if (valueDenominator.Type.IsMathematicalOperation())
        {
            // mathematical operation must be the same type as the initial operation (i.e. +- or *- is forbidden)
            if (operationToken.Type != valueDenominator.Type)
                throw new UnexpectedTokenException(valueDenominator, operationToken.Type);

            valueExpTokens = new Token[]
            {
                new(TokenType.LiteralNumber, "1", null),
            };
        }
        else
        {
            Debug.Assert(valueDenominator.Type is TokenType.EqualsSymbol);

            valueExpTokens = tokenQueue.DequeueUntil(TokenType.NewLine);
            tokenQueue.Dequeue(TokenType.NewLine);
        }

        // TODO: ugly. Move expression parsing to another class
        var expression = await ExpressionParser.ParseAsync(new[]
        {
            identifierToken, operationToken,
        }.Concat(valueExpTokens).ToArray(), cancellationToken);

        return new VariableAssignmentStatement(identifierToken, expression);
    }

    private async Task<Statement> ParseFunctionCall(Token identifierToken, CancellationToken cancellationToken = default)
    {
        // function call: <identifier>([<expression>[, <expression>]*])

        tokenQueue.Dequeue(TokenType.OpeningParentheses);
        var argsTokens = tokenQueue.DequeueUntil(TokenType.ClosingParentheses);
        tokenQueue.Dequeue(TokenType.ClosingParentheses);

        var args = await ExpressionParser.ParseExpressionsAsync(argsTokens, cancellationToken).ToListAsync(cancellationToken);

        return new FunctionCallStatement(identifierToken, args);
    }

    private async Task<Statement> ParseVariableDeclaration(Token typeToken, CancellationToken cancellationToken = default)
    {
        // variable declaration: <type name> <identifier> = <expression>

        var identifier = tokenQueue.Dequeue(TokenType.Identifier);

        if (!tokenQueue.TryPeekType(out var nextType) || nextType is TokenType.NewLine)
            return new VariableDeclarationStatement(typeToken, identifier, null);

        var equalsToken = tokenQueue.Dequeue(TokenType.EqualsSymbol);

        Expression expression;
        try
        {
            expression = await ParseValueExpression(cancellationToken);
        }
        catch (UnexpectedEndOfTokensException e)
        {
            throw new MissingValueExpressionException(equalsToken, e);
        }

        return new VariableDeclarationStatement(typeToken, identifier, expression);
    }

    private static IAsyncEnumerable<Statement> ParseStatements(IEnumerable<Token> tokens, CancellationToken cancellationToken)
    {
        // TODO: ugly? need to be able to parse tokens after they have been taken out of the queue

        var innerParser = new StatementParser();

        innerParser.Add(tokens);

        return innerParser.ParseAsync(cancellationToken);
    }
}