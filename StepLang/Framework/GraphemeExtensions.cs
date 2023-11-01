using System.Globalization;
using System.Text;

namespace StepLang.Framework;

internal static class GraphemeExtensions
{
    public static int GraphemeLength(this string str)
    {
        var enumerator = StringInfo.GetTextElementEnumerator(str);
        var count = 0;
        while (enumerator.MoveNext()) count++;
        return count;
    }

    public static string GraphemeSubstring(this string str, int start, int length)
    {
        if (length <= 0)
            return string.Empty;

        var subjectGraphemeLength = str.GraphemeLength();

        if (start < 0)
            start = subjectGraphemeLength + start;

        if (start < 0 || start >= subjectGraphemeLength)
            return string.Empty;

        length = Math.Min(length, subjectGraphemeLength - start);

        var enumerator = StringInfo.GetTextElementEnumerator(str);
        var count = 0;
        var startIndex = 0;
        var endIndex = 0;
        while (enumerator.MoveNext())
        {
            if (count == start)
                startIndex = enumerator.ElementIndex;

            if (count == start + length)
            {
                endIndex = enumerator.ElementIndex;

                break;
            }

            count++;
        }

        if (endIndex == 0)
            endIndex = str.Length;

        return str.Substring(startIndex, endIndex - startIndex);
    }

    public static IEnumerable<string> GraphemeSplit(this string str, string separator)
    {
        var enumerator = StringInfo.GetTextElementEnumerator(str);
        var separatorLength = separator.GraphemeLength();

        // yield all text elements one by one if the separator length is 0
        // otherwise check if the text element is the separator and yield all text elements up to the separator
        // then repeat until the end of the string

        var buffer = new StringBuilder();
        while (enumerator.MoveNext())
        {
            var current = enumerator.GetTextElement();
            if (separatorLength == 0)
            {
                yield return current;
            }
            else if (current == separator)
            {
                yield return buffer.ToString();
                buffer.Clear();
            }
            else
            {
                buffer.Append(current);
            }
        }

        if (buffer.Length > 0)
            yield return buffer.ToString();
    }

    public static string? GraphemeAt(this string str, int index)
    {
        if (index < 0)
            return null;

        var normalized = str.Normalize(NormalizationForm.FormD);
        var enumerator = StringInfo.GetTextElementEnumerator(normalized);
        var count = 0;
        while (enumerator.MoveNext())
        {
            if (count == index)
                return enumerator.GetTextElement();

            count++;
        }

        return null;
    }

    public static string GraphemeReplace(this string str, string search, string replacement)
    {
        var strNormalized = str.Normalize(NormalizationForm.FormD);
        var strNormalizedLength = strNormalized.GraphemeLength();
        var searchNormalized = search.Normalize(NormalizationForm.FormD);
        var searchNormalizedLength = searchNormalized.GraphemeLength();
        var replacementNormalized = replacement.Normalize(NormalizationForm.FormD);

        var sb = new StringBuilder();
        var i = 0;
        while (i < strNormalizedLength)
        {
            var current = strNormalized.GraphemeSubstring(i, searchNormalizedLength);
            if (current == searchNormalized)
            {
                sb.Append(replacementNormalized);
                i += searchNormalizedLength;
            }
            else
            {
                sb.Append(strNormalized.GraphemeAt(i));
                i++;
            }
        }

        return sb.ToString();
    }

    public static int GraphemeIndexOf(this string str, string value, int startIndex = 0)
    {
        var strNormalized = str.Normalize(NormalizationForm.FormD);
        var strNormalizedLength = strNormalized.GraphemeLength();
        var valueNormalized = value.Normalize(NormalizationForm.FormD);
        var valueNormalizedLength = valueNormalized.GraphemeLength();

        for (var i = startIndex; i < strNormalizedLength; i++)
        {
            var current = strNormalized.GraphemeSubstring(i, valueNormalizedLength);
            if (current == valueNormalized)
                return i;
        }

        return -1;
    }

    public static bool GraphemeStartsWith(this string str, string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var normalizedLength = normalized.GraphemeLength();

        var current = str.GraphemeSubstring(0, normalizedLength);
        return current == normalized;
    }

    public static bool GraphemeEndsWith(this string str, string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var normalizedLength = normalized.GraphemeLength();
        var valueLength = str.GraphemeLength();

        var current = str.GraphemeSubstring(valueLength - normalizedLength, normalizedLength);
        return current == normalized;
    }

    public static string ReverseGraphemes(this string str)
    {
        var enumerator = StringInfo.GetTextElementEnumerator(str);
        var count = 0;
        var graphemes = new string[str.GraphemeLength()];
        while (enumerator.MoveNext())
            graphemes[count++] = enumerator.GetTextElement();

        Array.Reverse(graphemes);

        return string.Concat(graphemes);
    }
}
