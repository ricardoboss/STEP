using System.Collections;

namespace StepLang.Tokenizing;

public class TokenCollection(IReadOnlyList<Token> tokens) : IEnumerable<Token>
{
    public IEnumerator<Token> GetEnumerator() => tokens.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public Token? At(int line, int column)
    {
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (token.Location.Line == line && token.Location.Column == column)
                return token;

            if (i < tokens.Count)
            {
                var nextToken = tokens.ElementAt(i + 1);
                if (nextToken.Location.Line == line && nextToken.Location.Column > column ||
                    nextToken.Location.Line > line)
                    return token;
            }
        }

        return null;
    }
}
