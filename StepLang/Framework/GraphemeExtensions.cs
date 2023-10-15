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

    public static string? GraphemeAt(this string str, int index)
    {
        if (index < 0)
            return null;

        var enumerator = StringInfo.GetTextElementEnumerator(str);
        var count = 0;
        while (enumerator.MoveNext())
        {
            if (count == index)
                return enumerator.GetTextElement();

            count++;
        }

        return null;
    }

    public static int GraphemeIndexOf(this string str, string value, int startIndex = 0)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var normalizedLength = normalized.GraphemeLength();

        for (var i = startIndex; i < normalizedLength; i++)
        {
            var current = str.GraphemeSubstring(i, normalizedLength);
            if (current == normalized)
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
}
