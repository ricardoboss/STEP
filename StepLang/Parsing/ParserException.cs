using StepLang.Tokenizing;

namespace StepLang.Parsing;

/// <summary>
/// Base class for all exceptions that occur during parsing.
/// </summary>
public abstract class ParserException : StepLangException
{
    /// <summary>
    /// Creates a new <see cref="ParserException"/> with the given error code, token, message, help text and inner exception.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <param name="token">The token that caused the exception, if any.</param>
    /// <param name="message">The message.</param>
    /// <param name="helpText">The help text.</param>
    /// <param name="inner">The inner exception, if any.</param>
    protected ParserException(int errorCode, Token? token, string message, string helpText, Exception? inner = null) : this(errorCode, token?.Location, message, helpText, inner)
    {
    }

    /// <summary>
    /// Creates a new <see cref="ParserException"/> with the given error code, location, message, help text and inner exception.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <param name="location">The location of the token that caused the exception, if any.</param>
    /// <param name="message">The message.</param>
    /// <param name="helpText">The help text.</param>
    /// <param name="inner">The inner exception, if any.</param>
    protected ParserException(int errorCode, TokenLocation? location, string message, string helpText, Exception? inner = null) : base($"PAR{errorCode:000}", location, message, helpText, inner)
    {
    }
}