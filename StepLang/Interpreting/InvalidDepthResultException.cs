using StepLang.Parsing.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class InvalidDepthResultException : InterpreterException
{
    private static string GetMessage(Token keywordToken, ExpressionResult result)
    {
        var message = $"Invalid {keywordToken.Value} depth: {keywordToken.Value} depth must be a positive number, got ";

        switch (result)
        {
            case StringResult stringResult:
                message += $"a string (\"{stringResult.Value}\")";
                break;
            case NumberResult numberResult:
                if (numberResult.Value == 0)
                    message += "0";
                else // all other values should be negative, as positive values wouldn't throw this exception
                    message += $"a negative number ({numberResult.Value})";
                break;
            default:
                message += result.ToString();
                break;
        }

        return message;
    }

    public InvalidDepthResultException(Token keywordToken, ExpressionResult result) : base(3, keywordToken.Location, GetMessage(keywordToken, result), "Make sure to only pass positive numbers as the depth to continue or break.")
    {
    }
}