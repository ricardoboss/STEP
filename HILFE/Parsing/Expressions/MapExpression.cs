﻿using System.Diagnostics.CodeAnalysis;
using HILFE.Interpreting;

namespace HILFE.Parsing.Expressions;

public class MapExpression : Expression
{
    private readonly IEnumerable<KeyValuePair<string, Expression>> map;

    public MapExpression(IEnumerable<KeyValuePair<string, Expression>> map)
    {
        this.map = map;
    }

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var pairs = await map.EvaluateAsync(interpreter, cancellationToken)
            .ToDictionaryAsync(
                p => p.Key,
                p => p.Value,
                cancellationToken
            );

        return ExpressionResult.Map(pairs);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugDisplay() => $"{{ {string.Join(", ", map.Select(e => e.Key + ": " + e.Value))} }}";
}