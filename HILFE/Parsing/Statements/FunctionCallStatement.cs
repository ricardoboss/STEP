using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class FunctionCallStatement : Statement, IExecutableStatement
{
    private readonly Token identifier;
    private readonly IReadOnlyList<Token> args;

    /// <inheritdoc />
    public FunctionCallStatement(Token identifier, IReadOnlyList<Token> args) : base(StatementType.FunctionCall)
    {
        if (identifier.Type != TokenType.Identifier)
            throw new UnexpectedTokenException(identifier, TokenType.Identifier);

        this.identifier = identifier;
        this.args = args;
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(Interpreter interpreter)
    {
        var expressions = new List<Expression>();
        for (var i = 0; i < args.Count; )
        {
            var expressionTokens = args.Skip(i).TakeWhile(t => t.Type != TokenType.ExpressionSeparator && t.Type != TokenType.ExpressionCloser).ToList();
            var expression = new Expression(expressionTokens);
            expressions.Add(expression);
            i += expressionTokens.Count + 1;
        }

        var callArgs = expressions.Select(e => e.Evaluate(interpreter)).ToList();

        var functionVariable = interpreter.CurrentScope.GetByIdentifier(identifier.Value);
        var functionDefiniton = functionVariable.Value as string;
        switch (functionDefiniton)
        {
            case "StdIn.ReadLine":
                var line = await interpreter.StdIn.ReadLineAsync();
                interpreter.CurrentScope.ParentScope?.AddIdentifier("$$RETURN", new("$$RETURN", "string", line));
                break;
            case "StdOut.Write":
                var stringArgs = callArgs.Select(a => a.Value?.ToString() ?? string.Empty).Cast<string>();
                await interpreter.StdOut.WriteAsync(string.Join("", stringArgs));
                break;
            default:
                throw new InterpreterException("Undefined function: " + functionDefiniton);
        }
    }
}