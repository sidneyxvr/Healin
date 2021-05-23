using System.Globalization;

namespace Healin.Shared.Utils
{
    public static class StringFormatter
    {
        public static string OnlyNumber(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var onlyNumber = string.Empty;
            foreach (var s in value)
            {
                if (char.IsDigit(s))
                {
                    onlyNumber += s;
                }
            }
            return onlyNumber.Trim();
        }

        public static string ToTitleCase(this string value)
        {
            var textInfo = new CultureInfo("pt-BR", false).TextInfo;
            return textInfo.ToTitleCase(value);
        }
    }
}
