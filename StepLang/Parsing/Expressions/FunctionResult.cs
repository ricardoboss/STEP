using StepLang.Parsing.Statements;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

public class FunctionResult : ValueExpressionResult<FunctionDefinition>
{
    public static FunctionResult VoidFunction => new(new UserDefinedFunctionDefinition(new List<(Token type, Token identifier)>(), new[] { new ReturnStatement(ConstantExpression.Void) }));

    /// <inheritdoc />
    public FunctionResult(FunctionDefinition value) : base(ResultType.Function, value)
    {
    }

    public override string ToString() => Value.ToString();
}