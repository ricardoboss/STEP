using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace StepLang.Tokenizing;

public class Tokenizer
{
    private readonly StringBuilder tokenBuilder = new();
    private readonly CharacterQueue characterQueue = new();

    private TokenLocation? stringStartLocation;
    private char? stringQuote;
    private bool escaped;

    public void Add(IEnumerable<char> characters) => characterQueue.Enqueue(characters);

    public void UpdateFile(FileSystemInfo file)
    {
        characterQueue.File = file;
    }

    public IEnumerable<Token> TokenizeAsync(CancellationToken cancellationToken = default)
    {
        while (characterQueue.TryDequeue(out var character))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (stringQuote.HasValue)
            {
                var token = HandleString(character);
                if (token is not null)
                    yield return token;

                continue;
            }

            if (character is '"' or '\'')
            {
                if (TryFinalizePreviousToken() is { } previousToken)
                    yield return previousToken;

                stringQuote = character;
                stringStartLocation = characterQueue.CurrentLocation;

                continue;
            }

            if (character == '/' && characterQueue.TryPeek(out var nextCharacter) && nextCharacter is '/')
            {
                foreach (var commentToken in HandleLineComment(character))
                    yield return commentToken;

                continue;
            }

            foreach (var token in HandleChar(character))
                yield return token;
        }

        if (stringQuote.HasValue)
            throw new UnterminatedStringException(stringStartLocation, stringQuote!.Value);

        if (tokenBuilder.Length == 0)
            yield break;

        var leftOverToken = TryFinalizeTokenFromBuilder(true);
        if (leftOverToken is not null) yield return leftOverToken;
    }

    private IEnumerable<Token> HandleLineComment(char character)
    {
        if (TryFinalizePreviousToken() is { } previousToken)
            yield return previousToken;

        tokenBuilder.Append(character);
        foreach (var commentCharacter in characterQueue.DequeueUntil('\n'))
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

    private Token? HandleString(char c)
    {
        if (c is ' ')
        {
            tokenBuilder.Append(c);

            escaped = false;
        }
        else if (c == stringQuote)
        {
            if (escaped)
            {
                tokenBuilder.Append(c);

                escaped = false;
            }
            else
            {
                stringQuote = null;
                stringStartLocation = null;

                return FinalizeToken(TokenType.LiteralString);
            }
        }
        else if (c is '\\' && !escaped)
            escaped = true;
        else
            tokenBuilder.Append(c);

        return null;
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

        if (tokenValue.IsKnownTypeName())
            return FinalizeToken(TokenType.TypeName);

        if (tokenValue.TryParseKeyword(out var tmpType))
            return FinalizeToken(tmpType.Value);

        if (double.TryParse(tokenValue, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
            return FinalizeToken(TokenType.LiteralNumber);

        if (bool.TryParse(tokenValue, out _))
            return FinalizeToken(TokenType.LiteralBoolean);

        if (!allowIdentifier)
            return null;

        if (tokenValue.IsValidIdentifier())
            return FinalizeToken(TokenType.Identifier);

        throw new InvalidIdentifierException(characterQueue.CurrentLocation, tokenValue);
    }

    private static bool IsPartOfLiteralNumber(char c)
    {
        return char.IsDigit(c) || c is '.' or '-' or '+';
    }

    private Token FinalizeToken(TokenType type)
    {
        var value = tokenBuilder.ToString();

        tokenBuilder.Clear();

        return new(type, value, characterQueue.CurrentLocation);
    }
}