using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace StepLang.Tokenizing;

public class Tokenizer
{
    private readonly StringBuilder tokenBuilder = new();
    private readonly CharacterQueue characterQueue = new();
    private readonly bool strict;

    private TokenLocation? stringStartLocation;
    private char? stringQuote;
    private bool escaped;

    public Tokenizer(bool strict = true)
    {
        this.strict = strict;
    }

    public void Add(IEnumerable<char> characters) => characterQueue.Enqueue(characters);

    public void UpdateFile(FileSystemInfo file)
    {
        characterQueue.File = file;
    }

    public IEnumerable<Token> Tokenize(CancellationToken cancellationToken = default)
    {
        while (characterQueue.TryDequeue(out var character))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (stringQuote.HasValue)
            {
                var tokens = HandleString(character);
                foreach (var token in tokens)
                    yield return token;

                continue;
            }

            if (character is '"')
            {
                if (TryFinalizePreviousToken() is { } previousToken)
                    yield return previousToken;

                tokenBuilder.Append(character);

                stringQuote = character;
                stringStartLocation = characterQueue.CurrentLocation;

                continue;
            }

            if (character is '/' && characterQueue.TryPeek(out var nextCharacter) && nextCharacter is '/')
            {
                foreach (var commentToken in HandleLineComment(character))
                    yield return commentToken;

                continue;
            }

            if (character is '\r' && characterQueue.TryPeek(out nextCharacter) && nextCharacter is '\n')
            {
                // skip \r in new lines
                continue;
            }

            foreach (var token in HandleChar(character))
                yield return token;
        }

        if (stringQuote.HasValue && strict)
            throw new UnterminatedStringException(stringStartLocation, stringQuote!.Value);

        if (tokenBuilder.Length == 0)
        {
            yield return new(TokenType.EndOfFile, "", characterQueue.CurrentLocation);

            yield break;
        }

        var leftOverToken = TryFinalizeTokenFromBuilder(true);
        if (leftOverToken is not null) yield return leftOverToken;

        yield return new(TokenType.EndOfFile, "", characterQueue.CurrentLocation);
    }

    private IEnumerable<Token> HandleLineComment(char character)
    {
        if (TryFinalizePreviousToken() is { } previousToken)
            yield return previousToken;

        tokenBuilder.Append(character);
        foreach (var commentCharacter in characterQueue.DequeueUntil('\n').TakeWhile(c => c is not '\r'))
            tokenBuilder.Append(commentCharacter);

        yield return FinalizeToken(TokenType.LineComment);
    }

    private Token? TryFinalizePreviousToken()
    {
        if (tokenBuilder.Length <= 0)
            return null;

        var token = TryFinalizeTokenFromBuilder(true);

        Debug.Assert(tokenBuilder.Length == 0);

        return token;
    }

    private IEnumerable<Token> HandleString(char c)
    {
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
            stringStartLocation = null;

            yield return FinalizeToken(TokenType.LiteralString);
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
                throw new UnterminatedStringException(stringStartLocation!, stringQuote!.Value);

            stringQuote = null;
            stringStartLocation = null;

            yield return FinalizeToken(TokenType.LiteralString);

            if (c is '\n')
                yield return new(TokenType.NewLine, c.ToString(), characterQueue.CurrentLocation);

            yield break;
        }

        tokenBuilder.Append(c);
    }

    private IEnumerable<Token> HandleChar(char c)
    {
        if (c.TryParseSymbol(out var symbolType))
        {
            if (TryFinalizePreviousToken() is { } previousToken)
                yield return previousToken;

            yield return new(symbolType.Value, c.ToString(), characterQueue.CurrentLocation);

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
        var nextCharAvailable = characterQueue.TryPeek(out var nextChar);
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

        if (!allowIdentifier)
            return null;

        if (tokenValue.IsValidIdentifier() || !strict)
            return FinalizeToken(TokenType.Identifier);

        throw new InvalidIdentifierException(characterQueue.CurrentLocation, tokenValue);
    }

    private static bool IsPartOfLiteralNumber(char c)
    {
        return char.IsDigit(c) || c is '.' or '-';
    }

    private Token FinalizeToken(TokenType type)
    {
        var value = tokenBuilder.ToString();

        tokenBuilder.Clear();

        return new(type, value, characterQueue.CurrentLocation);
    }
}