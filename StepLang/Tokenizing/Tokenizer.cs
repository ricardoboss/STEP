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
                stringQuote = character;
                stringStartLocation = characterQueue.CurrentLocation;

                continue;
            }

            if (character is '/' && characterQueue.TryPeek(out var nextCharacter) && nextCharacter is '/')
            {
                if (tokenBuilder.Length > 0)
                {
                    var tokenValue = tokenBuilder.ToString();
                    var token = HandleTokenValue(tokenValue);
                    if (token is not null)
                        yield return token;
                    else if (tokenValue.IsValidIdentifier())
                        yield return FinalizeToken(TokenType.Identifier);
                    else
                        throw new InvalidIdentifierException(characterQueue.CurrentLocation, tokenValue);

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

        var leftOverToken = HandleTokenValue(tokenBuilder.ToString());
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
        string tokenValue;

        if (c.TryParseSymbol(out var symbolType))
        {
            var tokens = new List<Token>();
        
            tokenValue = tokenBuilder.ToString();
            if (tokenValue.Length > 0)
            {
                var tempToken = HandleTokenValue(tokenValue);
                if (tempToken is not null)
                {
                    tokens.Add(tempToken);
                }
                else
                {
                    if (tokenValue.IsValidIdentifier())
                        tokens.Add(FinalizeToken(TokenType.Identifier));
                    else
                        throw new InvalidIdentifierException(characterQueue.CurrentLocation, tokenValue);
                }
            }

            tokens.Add(new(symbolType.Value, c.ToString(), characterQueue.CurrentLocation));

            return tokens.ToArray();
        }

        if (IsPartOfLiteralNumber(c))
        {
            tokenBuilder.Append(c);
            return null;
        }

        tokenValue = tokenBuilder.Append(c).ToString();
        var token = HandleTokenValue(tokenValue);
        if (token is null)
            return null;

        return new []
        {
            token,
        };
    }

    private Token? HandleTokenValue(string tokenValue)
    {
        if (tokenValue.IsKnownTypeName())
            return FinalizeToken(TokenType.TypeName);

        if (tokenValue.TryParseKeyword(out var tmpType))
            return FinalizeToken(tmpType.Value);

        if (double.TryParse(tokenValue, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
            return FinalizeToken(TokenType.LiteralNumber);

        if (bool.TryParse(tokenValue, out _))
            return FinalizeToken(TokenType.LiteralBoolean);

        return null;
    }

    private static bool IsPartOfLiteralNumber(char c)
    {
        return char.IsDigit(c) || c == '.' || c == '-' || c == '+';
    }

    private Token FinalizeToken(TokenType type)
    {
        var value = tokenBuilder.ToString();

        tokenBuilder.Clear();

        return new(type, value, characterQueue.CurrentLocation);
    }
}