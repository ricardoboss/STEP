using System.Globalization;
using System.Text;

namespace StepLang.Framework;

/// <summary>
/// A set of extension methods for working with graphemes.
/// </summary>
internal static class GraphemeExtensions
{
    /// <summary>
    /// Returns the number of graphemes in the string.
    /// </summary>
    /// <param name="str">The string to count graphemes in.</param>
    /// <returns>The number of graphemes in the string.</returns>
    public static int GraphemeLength(this string str)
    {
        var enumerator = StringInfo.GetTextElementEnumerator(str);
        var count = 0;
        while (enumerator.MoveNext()) count++;
        return count;
    }

    /// <summary>
    /// Returns a substring of the string, starting at the specified index and optionally with the specified length.
    /// </summary>
    /// <param name="str">The string to get a substring of.</param>
    /// <param name="start">The index to start at.</param>
    /// <param name="length">The length of the substring. If null, the substring will be from the start index to the end of the string.</param>
    /// <returns>The substring.</returns>
    /// <remarks>
    /// If the start index is negative, it will be treated as an index from the end of the string.
    /// If the length is negative or 0, an empty string will be returned.
    /// </remarks>
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

    /// <summary>
    /// Splits the string into an enumerable of graphemes, using the specified separator.
    /// </summary>
    /// <param name="str">The string to split.</param>
    /// <param name="separator">The separator to split on.</param>
    /// <returns>An enumerable of graphemes.</returns>
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

    /// <summary>
    /// Returns the grapheme at the specified index.
    /// </summary>
    /// <param name="str">The string to get a grapheme from.</param>
    /// <param name="index">The index of the grapheme to get.</param>
    /// <returns>The grapheme at the specified index, or null if the index is out of range.</returns>
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

    /// <summary>
    /// Returns the index of the first occurrence of the specified grapheme in the string, starting at the specified index.
    /// </summary>
    /// <param name="str">The string to search.</param>
    /// <param name="value">The grapheme to search for.</param>
    /// <param name="startIndex">The index to start searching at.</param>
    /// <returns>The index of the first occurrence of the grapheme, or -1 if it was not found.</returns>
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

    /// <summary>
    /// Returns whether the string starts with the specified prefix.
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <param name="value">The prefix to check for.</param>
    /// <returns>Whether the string starts with the specified prefix.</returns>
    public static bool GraphemeStartsWith(this string str, string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var normalizedLength = normalized.GraphemeLength();

        var current = str.GraphemeSubstring(0, normalizedLength);
        return current == normalized;
    }

    /// <summary>
    /// Returns whether the string ends with the specified suffix.
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <param name="value">The suffix to check for.</param>
    /// <returns>Whether the string ends with the specified suffix.</returns>
    public static bool GraphemeEndsWith(this string str, string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var normalizedLength = normalized.GraphemeLength();
        var valueLength = str.GraphemeLength();

        var current = str.GraphemeSubstring(valueLength - normalizedLength, normalizedLength);
        return current == normalized;
    }

    /// <summary>
    /// Reverses the graphemes in the string.
    /// </summary>
    /// <param name="str">The string to reverse.</param>
    /// <returns>The reversed string.</returns>
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
