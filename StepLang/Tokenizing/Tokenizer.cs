using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace StepLang.Tokenizing;

public class Tokenizer
{
    private readonly StringBuilder tokenBuilder = new();
    private readonly CharacterSource source;
    private readonly bool strict;

    private TokenLocation tokenStartLocation;
    private char? stringQuote;
    private bool escaped;
    private int? stringSourceLength;

    public Tokenizer(CharacterSource source, bool strict = true)
    {
        this.source = source;
        this.strict = strict;

        tokenStartLocation = GetCurrentLocation();
    }

    private bool InString => stringQuote.HasValue;

    private TokenLocation GetCurrentLocation()
    {
        var length = tokenBuilder.Length;
        if (InString && stringSourceLength is not null)
            length += stringSourceLength.Value;

        var column = source.Column - length;

        return new(source.File, source.Line, column, length);
    }

    public IEnumerable<Token> Tokenize(CancellationToken cancellationToken = default)
    {
        while (source.TryConsume(out var character))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (InString)
            {
                var tokens = HandleString(character);
                foreach (var token in tokens)
                    yield return token;

                continue;
            }

            if (character is '"')
            {
                foreach (var previous in TryFinalizePreviousAndStartNewToken(character))
                    yield return previous;

                stringQuote = character;

                continue;
            }

            if (character is '/' && source.TryPeek(out var nextCharacter) && nextCharacter is '/')
            {
                foreach (var commentToken in HandleLineComment(character))
                    yield return commentToken;

                continue;
            }

            if (character is '\r' && source.TryPeek(out nextCharacter) && nextCharacter is '\n')
            {
                // skip \r in new lines
                continue;
            }

            foreach (var token in HandleChar(character))
                yield return token;
        }

        if (stringQuote.HasValue && strict)
            throw new UnterminatedStringException(tokenStartLocation, stringQuote!.Value);

        if (tokenBuilder.Length == 0)
        {
            yield return FinalizeToken(TokenType.EndOfFile);

            yield break;
        }

        var leftOverToken = TryFinalizeTokenFromBuilder(true);
        if (leftOverToken is not null) yield return leftOverToken;

        yield return FinalizeToken(TokenType.EndOfFile);
    }

    private IEnumerable<Token> TryFinalizePreviousAndStartNewToken(char newTokenFirstChar)
    {
        var tokenEmitted = false;
        if (tokenBuilder.Length > 0 && TryFinalizeTokenFromBuilder(true) is { } token)
        {
            tokenEmitted = true;

            yield return token;
        }

        Debug.Assert(tokenBuilder.Length == 0);

        tokenBuilder.Append(newTokenFirstChar);

        if (tokenEmitted)
        {
            // the new tokens first char was already consumed, so we need to adjust the new token start location
            tokenStartLocation = tokenStartLocation with { Column = tokenStartLocation.Column - 1 };
        }
    }

    private IEnumerable<Token> HandleLineComment(char character)
    {
        foreach (var previous in TryFinalizePreviousAndStartNewToken(character))
            yield return previous;

        foreach (var commentCharacter in source.ConsumeUntil('\n').TakeWhile(c => c is not '\r'))
            tokenBuilder.Append(commentCharacter);

        yield return FinalizeToken(TokenType.LineComment);
    }

    private IEnumerable<Token> HandleString(char c)
    {
        stringSourceLength ??= 1; // assume the source already includes the quote character
        stringSourceLength++;

        if (escaped)
        {
            var escapedChar = $"\\{c}";

            tokenBuilder.Append(Regex.Unescape(escapedChar));

            escaped = false;

            yield break;
        }

        if (c == stringQuote)
        {
            tokenBuilder.Append(c);

            stringQuote = null;

            yield return FinalizeToken(TokenType.LiteralString);

            stringSourceLength = null;

            yield break;
        }

        if (c is '\\')
        {
            escaped = true;

            yield break;
        }

        // strings can't contain unescaped control characters
        if (char.IsControl(c))
        {
            if (strict)
                throw new UnterminatedStringException(tokenStartLocation, stringQuote!.Value);

            stringQuote = null;

            yield return FinalizeToken(TokenType.LiteralString);

            stringSourceLength = null;

            if (c is not '\n')
                yield break;

            tokenBuilder.Append(c);
            yield return FinalizeToken(TokenType.NewLine);

            yield break;
        }

        tokenBuilder.Append(c);
    }

    private IEnumerable<Token> HandleChar(char c)
    {
        if (c.TryParseSymbol(out var symbolType))
        {
            foreach (var previous in TryFinalizePreviousAndStartNewToken(c))
                yield return previous;

            yield return FinalizeToken(symbolType.Value);
            yield break;
        }

        tokenBuilder.Append(c);

        if (IsPartOfLiteralNumber(c))
            yield break;

        var token = TryFinalizeTokenFromBuilder(false);
        if (token is not null)
            yield return token;
    }

    private Token? TryFinalizeTokenFromBuilder(bool allowIdentifier)
    {
        var tokenValue = tokenBuilder.ToString();

        // keywords are only valid if they are not surrounded by alphanumeric characters
        var nextCharAvailable = source.TryPeek(out var nextChar);
        if (!nextCharAvailable || nextCharAvailable && !char.IsLetterOrDigit(nextChar))
        {
            if (tokenValue.IsKnownTypeName())
                return FinalizeToken(TokenType.TypeName);

            if (tokenValue.TryParseKeyword(out var tmpType))
                return FinalizeToken(tmpType.Value);
        }

        if (double.TryParse(tokenValue, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
            return FinalizeToken(TokenType.LiteralNumber);

        if (bool.TryParse(tokenValue, out _))
            return FinalizeToken(TokenType.LiteralBoolean);

        if (tokenValue.ToUpperInvariant() == "NULL")
            return FinalizeToken(TokenType.LiteralNull);

        if (!allowIdentifier)
            return null;

        if (tokenValue.IsValidIdentifier() || !strict)
            return FinalizeToken(TokenType.Identifier);

        throw new InvalidIdentifierException(GetCurrentLocation(), tokenValue);
    }

    private static bool IsPartOfLiteralNumber(char c)
    {
        return char.IsDigit(c) || c is '.' or '-';
    }

    private Token FinalizeToken(TokenType type)
    {
        var value = tokenBuilder.ToString();
        var token = new Token(type, value, tokenStartLocation with { Length = value.Length });

        // prepare for next token
        tokenBuilder.Clear();
        tokenStartLocation = GetCurrentLocation();

        return token;
    }
}