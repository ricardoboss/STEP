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
                if (tokenBuilder.Length > 0)
                {
                    var token = TryFinalizeTokenFromBuilder(true);
                    if (token is not null)
                        yield return token;

                    Debug.Assert(tokenBuilder.Length == 0);
                }

                stringQuote = character;
                stringStartLocation = characterQueue.CurrentLocation;

                continue;
            }

            if (character is '/' && characterQueue.TryPeek(out var nextCharacter) && nextCharacter is '/')
            {
                if (tokenBuilder.Length > 0)
                {
                    var token = TryFinalizeTokenFromBuilder(true);
                    if (token is not null)
                        yield return token;

                    Debug.Assert(tokenBuilder.Length == 0);
                }

                tokenBuilder.Append(character);
                foreach (var commentCharacter in characterQueue.DequeueUntil('\n'))
                    tokenBuilder.Append(commentCharacter);

                yield return FinalizeToken(TokenType.LineComment);

                continue;
            }

            var tokens = HandleChar(character);
            if (tokens is null)
                continue;

            foreach (var token in tokens)
                yield return token;
        }

        if (stringQuote.HasValue)
            throw new UnclosedStringException(stringStartLocation, stringQuote!.Value);

        if (tokenBuilder.Length == 0)
            yield break;

        var leftOverToken = TryFinalizeTokenFromBuilder(true);
        if (leftOverToken is not null) yield return leftOverToken;
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

    private Token []? HandleChar(char c)
    {
        if (c.TryParseSymbol(out var symbolType))
        {
            var tokens = new List<Token>();
        
            var tokenValue = tokenBuilder.ToString();
            if (tokenValue.Length > 0)
            {
                var tempToken = TryFinalizeTokenFromBuilder(true);
                if (tempToken is not null)
                    tokens.Add(tempToken);
            }

            tokens.Add(new(symbolType.Value, c.ToString(), characterQueue.CurrentLocation));

            return tokens.ToArray();
        }

        if (IsPartOfLiteralNumber(c))
        {
            tokenBuilder.Append(c);
            return null;
        }

        tokenBuilder.Append(c);
        var token = TryFinalizeTokenFromBuilder(false);
        if (token is null)
            return null;

        return new []
        {
            token,
        };
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