using System.Linq;

namespace Hummingbird.Cli.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsNotProvidedByUser(this string item)
        {
            return string.IsNullOrEmpty(item);
        }

        public static bool Exists(this string item)
        {
            return !string.IsNullOrEmpty(item);
        }

        public static int NumberOfOccurrencesOfSubstring(this string text, string substring)
        {
            int count = 0;
            int i = 0;
            while ((i = text.IndexOf(substring, i)) != -1)
            {
                i += substring.Length;
                count++;
            }

            return count;
        }
    }
}
