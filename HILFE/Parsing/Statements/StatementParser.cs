﻿using System.Diagnostics;
using System.Runtime.CompilerServices;
using HILFE.Parsing.Expressions;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class StatementParser
{
    private readonly TokenQueue tokenQueue = new();

    public void Add(Token token)
    {
        tokenQueue.Enqueue(token);
    }

    public void Add(IEnumerable<Token> tokens)
    {
        tokenQueue.Enqueue(tokens);
    }

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

            if (type is TokenType.TypeName)
                yield return await ParseVariableDeclaration(token, cancellationToken);
            else if (type is TokenType.Identifier or TokenType.UnderscoreSymbol)
                yield return await ParseIdentifierUsage(token, cancellationToken);
            else if (type is TokenType.IfKeyword)
                yield return await ParseIfStatement(cancellationToken);
            else if (type is TokenType.WhileKeyword)
                yield return await ParseWhileLoop(cancellationToken);
            else if (type is TokenType.ReturnKeyword)
                yield return await ParseReturnStatement(cancellationToken);
            else if (type is TokenType.OpeningCurlyBracket)
                yield return await ParseAnonymousCodeBlock(cancellationToken);
            else if (type is TokenType.ClosingCurlyBracket)
                yield break;
            else if (type is not TokenType.Whitespace and not TokenType.NewLine)
                throw new UnexpectedTokenException(token, TokenType.TypeName, TokenType.Identifier, TokenType.UnderscoreSymbol, TokenType.Whitespace, TokenType.NewLine, TokenType.IfKeyword, TokenType.WhileKeyword, TokenType.ReturnKeyword, TokenType.OpeningCurlyBracket, TokenType.ClosingCurlyBracket);
        }
    }

    private async Task<Statement> ParseReturnStatement(CancellationToken cancellationToken = default)
    {
        // return: return <expression>
        
        var expressionTokens = tokenQueue.ReadUntil(TokenType.NewLine);
        
        tokenQueue.Expect(TokenType.NewLine);
        
        var expression = await ExpressionParser.ParseAsync(expressionTokens, cancellationToken);
        
        return new ReturnStatement(expression);
    }

    private async Task<Statement> ParseAnonymousCodeBlock(CancellationToken cancellationToken)
    {
        // anonymous code block: { [statement]* }

        var statementsTokens = tokenQueue.ReadUntil(TokenType.ClosingCurlyBracket);
        tokenQueue.Expect(TokenType.ClosingCurlyBracket);

        var statements = await ParseStatements(statementsTokens, cancellationToken).ToListAsync(cancellationToken);

        return new AnonymousCodeBlockStatement(statements);
    }

    private async Task<Statement> ParseWhileLoop(CancellationToken cancellationToken)
    {
        // looping: while (<expression>) { [statement]* }

        tokenQueue.Expect(TokenType.OpeningParentheses);
        var expressionTokens = tokenQueue.ReadUntil(TokenType.ClosingParentheses);
        tokenQueue.Expect(TokenType.ClosingParentheses);

        tokenQueue.Expect(TokenType.OpeningCurlyBracket);
        var statementsTokens = tokenQueue.ReadUntil(TokenType.ClosingCurlyBracket);
        tokenQueue.Expect(TokenType.ClosingCurlyBracket);

        var condition = await ExpressionParser.ParseAsync(expressionTokens, cancellationToken);
        var statements = await ParseStatements(statementsTokens, cancellationToken).ToListAsync(cancellationToken);

        return new WhileStatement(condition, statements);
    }

    private async Task<Statement> ParseIfStatement(CancellationToken cancellationToken)
    {
        // branching: if (<expression>) { [statement]* } [else[if (<expression>)] { [statement]* } ]

        tokenQueue.Expect(TokenType.OpeningParentheses);
        var expressionTokens = tokenQueue.ReadUntil(TokenType.ClosingParentheses);
        tokenQueue.Expect(TokenType.ClosingParentheses);

        tokenQueue.Expect(TokenType.OpeningCurlyBracket);
        var trueBranchTokens = tokenQueue.ReadUntil(TokenType.ClosingCurlyBracket);
        tokenQueue.Expect(TokenType.ClosingCurlyBracket);

        var condition = await ExpressionParser.ParseAsync(expressionTokens, cancellationToken);
        var statements = await ParseStatements(trueBranchTokens, cancellationToken).ToListAsync(cancellationToken);

        if (tokenQueue.PeekType() is not TokenType.ElseKeyword)
            return new IfStatement(condition, statements);

        tokenQueue.Expect(TokenType.ElseKeyword);

        IReadOnlyList<Token>? elseExpressionTokens = null;
        if (tokenQueue.PeekType() is TokenType.IfKeyword)
        {
            tokenQueue.Expect(TokenType.OpeningParentheses);
            elseExpressionTokens = tokenQueue.ReadUntil(TokenType.ClosingParentheses);
            tokenQueue.Expect(TokenType.ClosingParentheses);
        }

        tokenQueue.Expect(TokenType.OpeningCurlyBracket);
        var falseBranchTokens = tokenQueue.ReadUntil(TokenType.ClosingCurlyBracket);
        tokenQueue.Expect(TokenType.ClosingCurlyBracket);

        var elseExpression =
            elseExpressionTokens is not null ? await ExpressionParser.ParseAsync(elseExpressionTokens, cancellationToken) : null;

        var elseStatements = await ParseStatements(falseBranchTokens, cancellationToken)
            .ToListAsync(cancellationToken);

        return new IfElseStatement(condition, statements, elseExpression, elseStatements);
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
        // variable assignment: <identifier> = <expression>

        tokenQueue.Expect(TokenType.EqualsSymbol);
        
        var expression = await ParseValueExpression(cancellationToken);

        if (identifierToken.Type is TokenType.UnderscoreSymbol)
            return new DiscardAssignmentStatement(expression);

        return new VariableAssignmentStatement(identifierToken, expression);
    }

    private async Task<Expression> ParseValueExpression(CancellationToken cancellationToken)
    {
        // FIXME: this does not work with multi-line values like function definitions
        var expressionTokens = tokenQueue.ReadUntil(TokenType.NewLine);

        tokenQueue.Expect(TokenType.NewLine);

        return await ExpressionParser.ParseAsync(expressionTokens, cancellationToken);
    }

    private async Task<Statement> ParseMathematicalOperation(Token identifierToken, CancellationToken cancellationToken = default)
    {
        // shortcut setter: <identifier>++ / <identifier>-- / <identifier> *= <expression> / <identifier> /= <expression>

        var operationToken = tokenQueue.Dequeue();
        var valueDenominator = tokenQueue.Expect(TokenType.PlusSymbol,
            TokenType.MinusSymbol, TokenType.AsteriskSymbol, TokenType.SlashSymbol,
            TokenType.PercentSymbol, TokenType.EqualsSymbol);

        IReadOnlyList<Token> valueExpTokens;
        if (valueDenominator.Type.IsMathematicalOperation())
        {
            // mathematical operation must be the same type as the initial operation (i.e. +- or *- is forbidden)
            if (operationToken.Type != valueDenominator.Type)
                throw new UnexpectedTokenException(valueDenominator, operationToken.Type);

            valueExpTokens = new Token []
            {
                new(TokenType.LiteralNumber, "1")
            };
        }
        else
        {
            Debug.Assert(valueDenominator.Type is TokenType.EqualsSymbol);

            valueExpTokens = tokenQueue.ReadUntil(TokenType.NewLine);
            tokenQueue.Expect(TokenType.NewLine);
        }

        // TODO: ugly. Move expression parsing to another class
        var expression = await ExpressionParser.ParseAsync(new []
        {
            identifierToken, operationToken,
        }.Concat(valueExpTokens).ToArray(), cancellationToken);

        return new VariableAssignmentStatement(identifierToken, expression);
    }

    private async Task<Statement> ParseFunctionCall(Token identifierToken, CancellationToken cancellationToken = default)
    {
        // function call: <identifier>([<expression>[, <expression>]*])

        tokenQueue.Expect(TokenType.OpeningParentheses);
        var argsTokens = tokenQueue.ReadUntil(TokenType.ClosingParentheses);
        tokenQueue.Expect(TokenType.ClosingParentheses);

        var args = await ExpressionParser.ParseExpressionsAsync(argsTokens, cancellationToken).ToListAsync(cancellationToken);

        return new FunctionCallStatement(identifierToken, args);
    }

    private async Task<Statement> ParseVariableDeclaration(Token typeToken, CancellationToken cancellationToken = default)
    {
        // variable declaration: <type name> <identifier> = <expression>

        var identifier = tokenQueue.Expect(TokenType.Identifier);

        tokenQueue.Expect(TokenType.EqualsSymbol);

        var expression = await ParseValueExpression(cancellationToken);
        
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