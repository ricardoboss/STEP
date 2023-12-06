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

    public static string GraphemeSubstring(this string str, int start, int? length = null)
    {
        if (length <= 0)
            return string.Empty;

        var subjectGraphemeLength = str.GraphemeLength();

        if (start < 0)
            start = subjectGraphemeLength + start;

        if (start < 0 || start >= subjectGraphemeLength)
            return string.Empty;

        length = Math.Min(length ?? int.MaxValue, subjectGraphemeLength - start);

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
        var strNormalized = str.Normalize(NormalizationForm.FormD);
        var separatorNormalized = separator.Normalize(NormalizationForm.FormD);

        if (string.IsNullOrEmpty(str))
        {
            yield return string.Empty;
            yield break;
        }

        if (string.IsNullOrEmpty(separatorNormalized))
        {
            var enumerator = StringInfo.GetTextElementEnumerator(str);
            while (enumerator.MoveNext())
                yield return enumerator.GetTextElement();

            yield break;
        }

        var startIdx = 0;
        var separatorIdx = strNormalized.GraphemeIndexOf(separatorNormalized);
        var separatorLength = separatorNormalized.GraphemeLength();

        while (separatorIdx != -1)
        {
            yield return strNormalized.GraphemeSubstring(startIdx, separatorIdx - startIdx);
            startIdx = separatorIdx + separatorLength;
            separatorIdx = strNormalized.GraphemeIndexOf(separatorNormalized, startIdx);
        }

        if (startIdx < strNormalized.GraphemeLength())
            yield return strNormalized.GraphemeSubstring(startIdx);
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
