using System;
using System.Collections.Generic;
using System.Text;

namespace vita.Utils
{
    public static class Base64Conversion
    {
        public static string Encode(string plainText)
        {
            plainText = RemoveStartingAndTrailingQuote(plainText);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        public static string Decode(string plainText)
        {
            plainText = RemoveStartingAndTrailingQuote(plainText);
            return Encoding.UTF8.GetString(Convert.FromBase64String(plainText));

        }
        private static string RemoveStartingAndTrailingQuote(string plainText)
        {
            if (plainText.StartsWith("\""))
                plainText = plainText.Remove(0, 1);
            if (plainText.EndsWith("\""))
            {
                plainText = plainText.Remove(plainText.Length - 1, 1);
            }

            return plainText;
        }
    }
}