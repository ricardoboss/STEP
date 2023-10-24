using System.Globalization;
using StepLang.Expressions.Results;
using StepLang.Framework;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Expressions;

public class IndexExpression : BinaryExpression
{
    public IndexExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Index)
    {
    }

    protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
    {
        switch (left.ResultType)
        {
            case ResultType.List:
            {
                var values = left.ExpectList().Value;
                var index = right.ExpectInteger().RoundedIntValue;

                if (index < 0 || index >= values.Count)
                    throw new IndexOutOfBoundsException(index, values.Count);

                return values[index];
            }
            case ResultType.Map:
            {
                var pairs = left.ExpectMap().Value;
                var key = right.ExpectString().Value;

                return pairs[key];
            }
            case ResultType.Str:
            {
                var value = left.ExpectString().Value;
                var index = right.ExpectInteger().RoundedIntValue;
                var grapheme = value.GraphemeAt(index);
                if (grapheme == null)
                    throw new IndexOutOfBoundsException(index, value.GraphemeLength());

                return new StringResult(grapheme);
            }
            default:
                var indexRepresentation = right switch
                {
                    StringResult stringResult => stringResult.Value,
                    NumberResult numberResult => numberResult.Value.ToString(CultureInfo.InvariantCulture),
                    _ => right.ToString(),
                };

                throw new InvalidIndexOperatorException(null, indexRepresentation, left.ResultType, "access");
        }
    }
}