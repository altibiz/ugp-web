using System;
using System.Globalization;
using System.Text;

namespace Members
{
    public class StringUtils
    {
        public static string StripDiacritics(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Returns true if the input contains any of the substrings in 'searchTerms', case-insensitive.
        /// </summary>
        public static bool ContainsAny(string input, params string[] searchTerms)
        {
            if (string.IsNullOrEmpty(input) || searchTerms == null || searchTerms.Length == 0)
                return false;

            foreach (var term in searchTerms)
            {
                if (!string.IsNullOrEmpty(term) &&
                    input.Contains(term, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
