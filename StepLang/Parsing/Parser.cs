using StepLang.Tokenizing;

namespace StepLang.Parsing;

public class Parser
{
    private readonly TokenQueue tokens;

    public Parser(IEnumerable<Token> tokens)
    {
        this.tokens = new(tokens, ignoreWhitespace: true);
    }

    public RootNode Parse()
    {
        var imports = new List<ImportNode>();
        while (tokens.TryPeekType(out var nextType) && nextType is TokenType.ImportKeyword)
            imports.Add(ParseImport());

        var statements = new List<StatementNode>();
        while (tokens.IsNotEmpty)
        {
            var node = ParseStatement();
            if (node is not null)
                statements.Add(node);
        }

        return new(imports, statements);
    }

    private ImportNode ParseImport()
    {
        _ = tokens.Dequeue(TokenType.ImportKeyword);
        var path = tokens.Dequeue(TokenType.LiteralString);

        if (tokens.TryPeekType(out var next) && next is TokenType.NewLine)
            _ = tokens.Dequeue(TokenType.NewLine);

        return new(path);
    }

    private StatementNode? ParseStatement()
    {
        var token = tokens.Peek();
        if (token.Type == TokenType.TypeName)
        {
            var next = tokens.Peek(1);
            if (next.Type == TokenType.Identifier)
            {
                if (tokens.Peek(2).Type == TokenType.EqualsSymbol)
                    return ParseVariableInitialization();

                return ParseVariableDeclaration();
            }

            if (next.Type == TokenType.QuestionMarkSymbol)
            {
                var nextNext = tokens.Peek(2);
                if (nextNext.Type == TokenType.Identifier)
                {
                    if (tokens.Peek(3).Type == TokenType.EqualsSymbol)
                        return ParseNullableVariableInitialization();

                    return ParseNullableVariableDeclaration();
                }

                throw new UnexpectedTokenException(nextNext, TokenType.Identifier);
            }

            throw new UnexpectedTokenException(next, TokenType.Identifier, TokenType.QuestionMarkSymbol);
        }

        if (token.Type == TokenType.Identifier)
        {
            var next = tokens.Peek(1);
            if (next.Type == TokenType.EqualsSymbol)
                return ParseVariableAssignment();

            if (next.Type == TokenType.OpeningParentheses)
                return ParseFunctionCall();

            if (next.Type.IsMathematicalOperation())
            {
                if (next.Type.IsShorthandMathematicalOperation() && tokens.Peek(2).Type == next.Type)
                    return ParseShorthandMathOperation();

                if (tokens.Peek(2).Type == TokenType.EqualsSymbol)
                    return ParseShorthandMathOperationExpression();

                throw new UnexpectedTokenException(tokens.Peek(2), next.Type, TokenType.EqualsSymbol);
            }

            if (next.Type == TokenType.OpeningSquareBracket)
                return ParseIndexAssignment();

            throw new UnexpectedTokenException(token, TokenType.EqualsSymbol, TokenType.OpeningParentheses);
        }

        if (token.Type is TokenType.NewLine or TokenType.LineComment)
        {
            _ = tokens.Dequeue(TokenType.NewLine, TokenType.LineComment);
            return null;
        }

        if (token.Type == TokenType.IfKeyword)
            return ParseIfStatement();

        if (token.Type == TokenType.WhileKeyword)
            return ParseWhileStatement();

        if (token.Type == TokenType.ForEachKeyword)
            return ParseForeachStatement();

        if (token.Type == TokenType.ReturnKeyword)
            return ParseReturnStatement();

        if (token.Type == TokenType.BreakKeyword)
            return ParseBreakStatement();

        if (token.Type == TokenType.ContinueKeyword)
            return ParseContinueStatement();

        if (token.Type == TokenType.OpeningCurlyBracket)
            return ParseCodeBlock();

        throw new UnexpectedTokenException(token, TokenType.TypeName, TokenType.Identifier, TokenType.NewLine, TokenType.LineComment);
    }

    private IdentifierIndexAssignmentNode ParseIndexAssignment()
    {
        var identifier = tokens.Dequeue(TokenType.Identifier);

        _ = tokens.Dequeue(TokenType.OpeningSquareBracket);

        var index = ParseExpression();

        _ = tokens.Dequeue(TokenType.ClosingSquareBracket);
        _ = tokens.Dequeue(TokenType.EqualsSymbol);

        var expression = ParseExpression();

        return new(identifier, index, expression);
    }

    private ShorthandMathOperationStatementNode ParseShorthandMathOperation()
    {
        var identifier = tokens.Dequeue(TokenType.Identifier);
        var operation = tokens.Dequeue(TokenTypes.ShorthandMathematicalOperations);
        _ = tokens.Dequeue(operation.Type);

        return new(identifier, operation);
    }

    private ShorthandMathOperationExpressionStatementNode ParseShorthandMathOperationExpression()
    {
        var identifier = tokens.Dequeue(TokenType.Identifier);
        var operation = tokens.Dequeue(TokenTypes.MathematicalOperations);
        _ = tokens.Dequeue(TokenType.EqualsSymbol);
        var expression = ParseExpression();

        return new(identifier, operation, expression);
    }

    private StatementNode ParseForeachStatement()
    {
        _ = tokens.Dequeue(TokenType.ForEachKeyword);
        _ = tokens.Dequeue(TokenType.OpeningParentheses);

        Token? keyType = null;
        Token? keyIdentifier = null;
        Token? valueType = null;
        Token valueIdentifier;

        var next = tokens.Peek();
        if (next.Type == TokenType.TypeName)
        {
            var firstType = tokens.Dequeue(TokenType.TypeName);
            var firstIdentifier = tokens.Dequeue(TokenType.Identifier);

            next = tokens.Peek();
            if (next.Type == TokenType.ColonSymbol)
            {
                keyType = firstType;
                keyIdentifier = firstIdentifier;

                _ = tokens.Dequeue(TokenType.ColonSymbol);

                next = tokens.Peek();
                if (next.Type == TokenType.TypeName)
                {
                    valueType = tokens.Dequeue(TokenType.TypeName);
                    valueIdentifier = tokens.Dequeue(TokenType.Identifier);
                }
                else if (next.Type == TokenType.Identifier)
                {
                    valueType = null;
                    valueIdentifier = tokens.Dequeue(TokenType.Identifier);
                }
                else
                {
                    throw new UnexpectedTokenException(next, TokenType.TypeName, TokenType.Identifier);
                }
            }
            else if (next.Type == TokenType.InKeyword)
            {
                valueType = firstType;
                valueIdentifier = firstIdentifier;
            }
            else
            {
                throw new UnexpectedTokenException(next, TokenType.ColonSymbol, TokenType.InKeyword);
            }
        }
        else if (next.Type == TokenType.Identifier)
        {
            var firstIdentifier = tokens.Dequeue(TokenType.Identifier);

            next = tokens.Peek();
            if (next.Type == TokenType.ColonSymbol)
            {
                keyType = null;
                keyIdentifier = firstIdentifier;

                _ = tokens.Dequeue(TokenType.ColonSymbol);

                next = tokens.Peek();
                if (next.Type == TokenType.TypeName)
                {
                    valueType = tokens.Dequeue(TokenType.TypeName);
                    valueIdentifier = tokens.Dequeue(TokenType.Identifier);
                }
                else if (next.Type == TokenType.Identifier)
                {
                    valueType = null;
                    valueIdentifier = tokens.Dequeue(TokenType.Identifier);
                }
                else
                {
                    throw new UnexpectedTokenException(next, TokenType.TypeName, TokenType.Identifier);
                }
            }
            else if (next.Type == TokenType.InKeyword)
            {
                valueIdentifier = firstIdentifier;
            }
            else
            {
                throw new UnexpectedTokenException(next, TokenType.ColonSymbol, TokenType.InKeyword);
            }
        }
        else
        {
            throw new UnexpectedTokenException(next, TokenType.TypeName, TokenType.Identifier);
        }

        _ = tokens.Dequeue(TokenType.InKeyword);

        var list = ParseExpression();

        _ = tokens.Dequeue(TokenType.ClosingParentheses);

        var body = ParseCodeBlock().Body;

        if (keyType is not null && keyIdentifier is not null && valueType is not null)
            return new ForeachDeclareKeyDeclareValueStatementNode(keyType, keyIdentifier, valueType, valueIdentifier, list, body);

        if (keyType is not null && keyIdentifier is not null && valueType is null)
            return new ForeachDeclareKeyValueStatementNode(keyType, keyIdentifier, valueIdentifier, list, body);

        if (keyType is null && keyIdentifier is not null && valueType is not null)
            return new ForeachKeyDeclareValueStatementNode(keyIdentifier, valueType, valueIdentifier, list, body);

        if (keyType is null && keyIdentifier is not null && valueType is null)
            return new ForeachKeyValueStatementNode(keyIdentifier, valueIdentifier, list, body);

        if (keyType is null && keyIdentifier is null && valueType is not null)
            return new ForeachDeclareValueStatementNode(valueType, valueIdentifier, list, body);

        // keyType is null && keyIdentifier is null && valueType is null
        return new ForeachValueStatementNode(valueIdentifier, list, body);
    }

    private CodeBlockStatementNode ParseCodeBlock()
    {
        _ = tokens.Dequeue(TokenType.OpeningCurlyBracket);

        var statements = new List<StatementNode>();
        while (tokens.TryPeek(out var next) && next.Type != TokenType.ClosingCurlyBracket)
        {
            var statement = ParseStatement();
            if (statement is not null)
                statements.Add(statement);
        }

        _ = tokens.Dequeue(TokenType.ClosingCurlyBracket);

        return new(statements);
    }

    private ContinueStatementNode ParseContinueStatement()
    {
        var continueToken = tokens.Dequeue(TokenType.ContinueKeyword);

        return new(continueToken);
    }

    private BreakStatementNode ParseBreakStatement()
    {
        var breakToken = tokens.Dequeue(TokenType.BreakKeyword);

        return new(breakToken);
    }

    private ReturnStatementNode ParseReturnStatement()
    {
        _ = tokens.Dequeue(TokenType.ReturnKeyword);
        var expression = ParseExpression();
        return new(expression);
    }

    private WhileStatementNode ParseWhileStatement()
    {
        _ = tokens.Dequeue(TokenType.WhileKeyword);
        _ = tokens.Dequeue(TokenType.OpeningParentheses);
        var condition = ParseExpression();
        _ = tokens.Dequeue(TokenType.ClosingParentheses);
        var codeBlock = ParseCodeBlock();
        return new(condition, codeBlock.Body);
    }

    private StatementNode ParseIfStatement()
    {
        _ = tokens.Dequeue(TokenType.IfKeyword);
        _ = tokens.Dequeue(TokenType.OpeningParentheses);
        var condition = ParseExpression();
        _ = tokens.Dequeue(TokenType.ClosingParentheses);
        var codeBlock = ParseCodeBlock();

        if (tokens.TryPeek(out var next) && next.Type == TokenType.ElseKeyword)
        {
            _ = tokens.Dequeue(TokenType.ElseKeyword);
            if (tokens.TryPeek(out next) && next.Type == TokenType.IfKeyword)
            {
                _ = tokens.Dequeue(TokenType.IfKeyword);
                _ = tokens.Dequeue(TokenType.OpeningParentheses);
                var elseCondition = ParseExpression();
                _ = tokens.Dequeue(TokenType.ClosingParentheses);
                var elseIfCodeBlock = ParseCodeBlock();
                return new IfElseIfStatementNode(condition, codeBlock.Body, elseCondition, elseIfCodeBlock.Body);
            }

            var elseCodeBlock = ParseCodeBlock();
            return new IfElseStatementNode(condition, codeBlock.Body, elseCodeBlock.Body);
        }

        return new IfStatementNode(condition, codeBlock.Body);
    }

    private CallStatementNode ParseFunctionCall()
    {
        var identifier = tokens.Dequeue(TokenType.Identifier);
        _ = tokens.Dequeue(TokenType.OpeningParentheses);
        var arguments = ParseArguments();
        _ = tokens.Dequeue(TokenType.ClosingParentheses);
        return new(identifier, arguments);
    }

    private VariableDeclarationNode ParseVariableDeclaration()
    {
        var type = tokens.Dequeue(TokenType.TypeName);
        var identifier = tokens.Dequeue(TokenType.Identifier);
        return new(new[] { type }, identifier);
    }

    private NullableVariableDeclarationNode ParseNullableVariableDeclaration()
    {
        var type = tokens.Dequeue(TokenType.TypeName);
        var nullabilityIndicator = tokens.Dequeue(TokenType.QuestionMarkSymbol);
        var identifier = tokens.Dequeue(TokenType.Identifier);
        return new(new[] { type }, nullabilityIndicator, identifier);
    }

    private VariableInitializationNode ParseVariableInitialization()
    {
        var type = tokens.Dequeue(TokenType.TypeName);
        var identifier = tokens.Dequeue(TokenType.Identifier);
        _ = tokens.Dequeue(TokenType.EqualsSymbol);
        var expression = ParseExpression();
        return new(new[] { type }, identifier, expression);
    }

    private NullableVariableInitializationNode ParseNullableVariableInitialization()
    {
        var type = tokens.Dequeue(TokenType.TypeName);
        var nullabilityIndicator = tokens.Dequeue(TokenType.QuestionMarkSymbol);
        var identifier = tokens.Dequeue(TokenType.Identifier);
        _ = tokens.Dequeue(TokenType.EqualsSymbol);
        var expression = ParseExpression();
        return new(new[] { type }, nullabilityIndicator, identifier, expression);
    }

    private VariableAssignmentNode ParseVariableAssignment()
    {
        var identifier = tokens.Dequeue(TokenType.Identifier);
        _ = tokens.Dequeue(TokenType.EqualsSymbol);
        var expression = ParseExpression();
        return new(identifier, expression);
    }

    private ExpressionNode ParseExpression()
    {
        var left = new PrimaryExpressionNode(ParsePrimaryExpression());
        if (!tokens.TryPeek(out var next) || !next.Type.IsOperator())
            return left;

        var op = ParseOperatorExpression();
        var right = ParseExpression();

        switch (op.Operator)
        {
            case BinaryExpressionOperator.Add:
                return new AddExpressionNode(left, right);
            case BinaryExpressionOperator.Coalesce:
                return new CoalesceExpressionNode(left, right);
            case BinaryExpressionOperator.NotEqual:
                return new NotEqualsExpressionNode(left, right);
            case BinaryExpressionOperator.Equal:
                return new EqualsExpressionNode(left, right);
            default:
                throw new NotImplementedException("Expression for operator " + op.Operator + " not implemented");
        }
    }

    private BinaryOperatorNode ParseOperatorExpression()
    {
        var operators = new List<Token>();
        while (tokens.TryPeek(out var next))
        {
            if (next.Type.IsOperator())
                operators.Add(tokens.Dequeue());
            else if (operators.Count == 0)
                throw new UnexpectedTokenException(next, TokenTypes.Operators);
            else
                break;
        }

        BinaryExpressionOperator @operator;
        switch (operators.Count)
        {
            case 3:
            {
                var (first, second, third) = (operators[0], operators[1], operators[2]);

                @operator = first.Type switch
                {
                    TokenType.GreaterThanSymbol => second.Type switch
                    {
                        TokenType.GreaterThanSymbol => third.Type switch
                        {
                            TokenType.GreaterThanSymbol => BinaryExpressionOperator.BitwiseRotateRight,
                            _ => throw new UnexpectedTokenException(third, TokenType.GreaterThanSymbol),
                        },
                        _ => throw new UnexpectedTokenException(second, TokenType.GreaterThanSymbol),
                    },
                    TokenType.LessThanSymbol => second.Type switch
                    {
                        TokenType.LessThanSymbol => third.Type switch
                        {
                            TokenType.LessThanSymbol => BinaryExpressionOperator.BitwiseRotateLeft,
                            _ => throw new UnexpectedTokenException(third, TokenType.LessThanSymbol),
                        },
                        _ => throw new UnexpectedTokenException(second, TokenType.LessThanSymbol),
                    },
                    _ => throw new UnexpectedTokenException(first, TokenType.GreaterThanSymbol, TokenType.LessThanSymbol),
                };

                break;
            }
            case 2:
            {
                var (first, second) = (operators[0], operators[1]);

                @operator = first.Type switch
                {
                    TokenType.AsteriskSymbol => second.Type switch
                    {
                        TokenType.AsteriskSymbol => BinaryExpressionOperator.Power,
                        _ => throw new UnexpectedTokenException(second, TokenType.AsteriskSymbol),
                    },
                    TokenType.EqualsSymbol => second.Type switch
                    {
                        TokenType.EqualsSymbol => BinaryExpressionOperator.Equal,
                        _ => throw new UnexpectedTokenException(second, TokenType.EqualsSymbol),
                    },
                    TokenType.ExclamationMarkSymbol => second.Type switch
                    {
                        TokenType.EqualsSymbol => BinaryExpressionOperator.NotEqual,
                        _ => throw new UnexpectedTokenException(second, TokenType.EqualsSymbol),
                    },
                    TokenType.AmpersandSymbol => second.Type switch
                    {
                        TokenType.AmpersandSymbol => BinaryExpressionOperator.LogicalAnd,
                        _ => throw new UnexpectedTokenException(second, TokenType.AmpersandSymbol),
                    },
                    TokenType.PipeSymbol => second.Type switch
                    {
                        TokenType.PipeSymbol => BinaryExpressionOperator.LogicalOr,
                        _ => throw new UnexpectedTokenException(second, TokenType.PipeSymbol),
                    },
                    TokenType.LessThanSymbol => second.Type switch
                    {
                        TokenType.EqualsSymbol => BinaryExpressionOperator.LessThanOrEqual,
                        TokenType.LessThanSymbol => BinaryExpressionOperator.BitwiseShiftLeft,
                        _ => throw new UnexpectedTokenException(second, TokenType.EqualsSymbol, TokenType.LessThanSymbol),
                    },
                    TokenType.GreaterThanSymbol => second.Type switch
                    {
                        TokenType.EqualsSymbol => BinaryExpressionOperator.GreaterThanOrEqual,
                        TokenType.GreaterThanSymbol => BinaryExpressionOperator.BitwiseShiftRight,
                        _ => throw new UnexpectedTokenException(second, TokenType.EqualsSymbol, TokenType.GreaterThanSymbol),
                    },
                    TokenType.QuestionMarkSymbol => second.Type switch
                    {
                        TokenType.QuestionMarkSymbol => BinaryExpressionOperator.Coalesce,
                        _ => throw new UnexpectedTokenException(second, TokenType.QuestionMarkSymbol),
                    },
                    _ => throw new UnexpectedTokenException(first, TokenType.AsteriskSymbol, TokenType.EqualsSymbol, TokenType.ExclamationMarkSymbol, TokenType.AmpersandSymbol, TokenType.PipeSymbol, TokenType.LessThanSymbol, TokenType.GreaterThanSymbol, TokenType.QuestionMarkSymbol),
                };

                break;
            }
            case 1:
            {
                var first = operators[0];

                @operator = first.Type switch
                {
                    TokenType.PlusSymbol => BinaryExpressionOperator.Add,
                    TokenType.MinusSymbol => BinaryExpressionOperator.Subtract,
                    TokenType.AsteriskSymbol => BinaryExpressionOperator.Multiply,
                    TokenType.SlashSymbol => BinaryExpressionOperator.Divide,
                    TokenType.PercentSymbol => BinaryExpressionOperator.Modulo,
                    TokenType.GreaterThanSymbol => BinaryExpressionOperator.GreaterThan,
                    TokenType.LessThanSymbol => BinaryExpressionOperator.LessThan,
                    TokenType.PipeSymbol => BinaryExpressionOperator.BitwiseOr,
                    TokenType.AmpersandSymbol => BinaryExpressionOperator.BitwiseAnd,
                    TokenType.HatSymbol => BinaryExpressionOperator.BitwiseXor,
                    _ => throw new UnexpectedTokenException(first, TokenType.PlusSymbol, TokenType.MinusSymbol, TokenType.AsteriskSymbol, TokenType.SlashSymbol, TokenType.PercentSymbol, TokenType.GreaterThanSymbol, TokenType.LessThanSymbol, TokenType.PipeSymbol, TokenType.AmpersandSymbol, TokenType.HatSymbol),
                };

                break;
            }
            case 0:
                throw new UnexpectedEndOfTokensException(tokens.LastToken?.Location, "Expected an operator");
            default:
                throw new UnexpectedTokenException(operators[0], "Operators can only be chained up to 3 times");
        }

        return new(operators, @operator);
    }

    private ExpressionNode ParsePrimaryExpression()
    {
        var token = tokens.Dequeue();
        if (token.Type.IsLiteral())
            return new LiteralExpressionNode(token);

        switch (token.Type)
        {
            case TokenType.OpeningParentheses:
            {
                if (tokens.TryPeekType(out var nextType) && nextType == TokenType.TypeName)
                {
                    // function declaration
                    var arguments = new List<IVariableDeclarationNode>();
                    while (tokens.TryPeek(out var next) && next.Type != TokenType.ClosingParentheses)
                    {
                        var type = tokens.Dequeue(TokenType.TypeName);
                        Token? nullableIndicator = null;
                        if (tokens.TryPeek(out next) && next.Type is TokenType.QuestionMarkSymbol)
                            nullableIndicator = tokens.Dequeue(TokenType.QuestionMarkSymbol);

                        var identifier = tokens.Dequeue(TokenType.Identifier);

                        arguments.Add(nullableIndicator is not null ?
                            new NullableVariableDeclarationNode(new[] { type }, nullableIndicator, identifier) :
                            new VariableDeclarationNode(new[] { type }, identifier));

                        if (tokens.TryPeek(out next) && next.Type == TokenType.CommaSymbol)
                            _ = tokens.Dequeue(TokenType.CommaSymbol);
                    }

                    _ = tokens.Dequeue(TokenType.ClosingParentheses);

                    var body = ParseCodeBlock().Body;

                    return new FunctionDefinitionExpressionNode(arguments, body);
                }

                var expression = ParseExpression();

                _ = tokens.Dequeue(TokenType.ClosingParentheses);

                return expression;
            }
            case TokenType.Identifier:
            {
                var next = tokens.Peek();
                if (next.Type == TokenType.OpeningParentheses)
                {
                    _ = tokens.Dequeue(TokenType.OpeningParentheses);
                    var arguments = ParseArguments();
                    _ = tokens.Dequeue(TokenType.ClosingParentheses);
                    return new CallExpressionNode(token, arguments);
                }

                if (next.Type == TokenType.OpeningSquareBracket)
                {
                    _ = tokens.Dequeue(TokenType.OpeningSquareBracket);
                    var index = ParseExpression();
                    _ = tokens.Dequeue(TokenType.ClosingSquareBracket);
                    return new IdentifierIndexAccessExpressionNode(token, index);
                }

                return new IdentifierExpressionNode(token);
            }
            case TokenType.OpeningSquareBracket:
            {
                var list = new List<ExpressionNode>();
                while (tokens.TryPeek(out var next) && next.Type != TokenType.ClosingSquareBracket)
                {
                    list.Add(ParseExpression());
                    if (tokens.TryPeek(out next) && next.Type == TokenType.CommaSymbol)
                        _ = tokens.Dequeue(TokenType.CommaSymbol);
                }

                _ = tokens.Dequeue(TokenType.ClosingSquareBracket);

                return new ListExpressionNode(list);
            }
            case TokenType.OpeningCurlyBracket:
            {
                var map = new Dictionary<Token, ExpressionNode>();
                while (tokens.TryPeek(out var next) && next.Type != TokenType.ClosingCurlyBracket)
                {
                    var key = tokens.Dequeue(TokenType.LiteralString);
                    _ = tokens.Dequeue(TokenType.ColonSymbol);
                    var value = ParseExpression();
                    map.Add(key, value);
                    if (tokens.TryPeek(out next) && next.Type == TokenType.CommaSymbol)
                        _ = tokens.Dequeue(TokenType.CommaSymbol);
                }

                _ = tokens.Dequeue(TokenType.ClosingCurlyBracket);

                return new MapExpressionNode(map);
            }
            case TokenType.MinusSymbol:
            {
                var expression = ParseExpression();
                return new NegateExpressionNode(expression);
            }
            default:
                throw new NotImplementedException("Primary expression with token type " + token.Type + " not implemented");
        }
    }

    private List<ExpressionNode> ParseArguments()
    {
        var arguments = new List<ExpressionNode>();
        while (tokens.TryPeek(out var next) && next.Type != TokenType.ClosingParentheses)
        {
            arguments.Add(ParseExpression());
            if (tokens.TryPeek(out next) && next.Type == TokenType.CommaSymbol)
                _ = tokens.Dequeue();
        }

        return arguments;
    }
}