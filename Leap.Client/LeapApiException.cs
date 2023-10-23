namespace Leap.Client;

public class LeapApiException : Exception
{
    public LeapApiException(string message, Exception inner) : base(message, inner)
    {
    }
}