using StepLang.Tokenizing;

namespace StepLang;

/// <summary>
/// This is the base class for all exceptions thrown by STEP, which provide additional information about the source
/// location, an error code and a help text.
/// </summary>
public abstract class StepLangException : Exception
{
    /// <summary>
    /// The help text for this exception, which contains additional information and suggestions where the problem source
    /// might be and how to solve it.
    /// </summary>
    /// <example>
    /// "A string is missing a '"' (double quote). Strings must be closed with a matching delimiter."
    /// </example>
    /// <returns>
    /// The help text for this exception.
    /// </returns>
    public string HelpText { get; }

    /// <summary>
    /// The location of the token that caused this exception, if available.
    /// </summary>
    /// <returns>
    /// A <see cref="TokenLocation"/> object containing information about the file, line and column of the error causing
    /// token, or <see langword="null"/> if no location is available.
    /// </returns>
    public TokenLocation? Location { get; }

    /// <summary>
    /// <para>An error code in the form "xxxyyy", where "xxx" is the error group and "yyy" is the error number.</para>
    /// <para>
    /// For each error code, there exists a wiki page with additional information and suggestions on how to solve the
    /// problem.
    /// </para>
    /// </summary>
    /// <example>
    /// "TOK002" is the error code for an "Unterminated String Exception".
    /// </example>
    public string ErrorCode { get; }

    /// <summary>
    /// Creates a new instance of <see cref="StepLangException"/>.
    /// <para>
    /// The <see cref="Exception.HelpLink"/> property is automatically set to the wiki page for the given error code.
    /// </para>
    /// </summary>
    /// <param name="errorCode">
    /// The error code for this exception in the form "xxxyyy". See <see cref="ErrorCode"/>
    /// </param>
    /// <param name="location">
    /// The location of the token that caused this exception, if available. See <see cref="Location"/>
    /// </param>
    /// <param name="message">
    /// The message for this exception. See <see cref="Exception.Message"/>
    /// </param>
    /// <param name="helpText">
    /// The help text for this exception. See <see cref="HelpText"/>
    /// </param>
    /// <param name="innerException">
    /// The inner exception for this exception, if any. See <see cref="Exception.InnerException"/>
    /// </param>
    protected StepLangException(string errorCode, TokenLocation? location, string message, string helpText,
        Exception? innerException) : base(message, innerException)
    {
        HelpText = helpText;
        Location = location;
        ErrorCode = errorCode;

        // ReSharper disable once VirtualMemberCallInConstructor
        HelpLink = $"https://github.com/ricardoboss/STEP/wiki/{ErrorCode}";
    }
}