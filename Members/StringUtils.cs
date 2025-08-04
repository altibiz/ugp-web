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
    }
}
