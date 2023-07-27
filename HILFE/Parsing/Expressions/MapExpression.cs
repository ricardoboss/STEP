using System.Collections.Immutable;
using HILFE.Interpreting;

namespace HILFE.Parsing.Expressions;

public class MapExpression : Expression
{
    public MapExpression(ImmutableSortedDictionary<string,ExpressionResult> map)
    {
        throw new NotImplementedException();
    }

    public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}