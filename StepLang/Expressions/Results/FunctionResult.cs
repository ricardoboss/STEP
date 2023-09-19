using StepLang.Statements;
using StepLang.Tokenizing;

namespace StepLang.Expressions.Results;

public class FunctionResult : ValueExpressionResult<FunctionDefinition>
{
    public static FunctionResult VoidFunction => new UserDefinedFunctionDefinition(new List<(Token type, Token identifier)>(), new[] { new ReturnStatement(ConstantExpression.Void) }).ToResult();

    /// <inheritdoc />
    public FunctionResult(FunctionDefinition value) : base(ResultType.Function, value)
    {
    }

    protected override bool EqualsInternal(ExpressionResult other)
    {
        return other is FunctionResult functionResult && ReferenceEquals(Value, functionResult.Value);
    }

    public override FunctionResult DeepClone() => Value.ToResult();
}