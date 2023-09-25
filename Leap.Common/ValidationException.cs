namespace Leap.Common;

public class ValidationException : InvalidOperationException
{
    public ValidationException(string message) : base(message)
    {
    }
}