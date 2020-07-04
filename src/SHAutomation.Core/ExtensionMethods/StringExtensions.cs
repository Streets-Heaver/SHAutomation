using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SHAutomation.Core.ExtensionMethods
{
    public static class stringExtensions
    {
        /// <summary>
        /// Changes first letter of each string (spaced) to upper case
        /// </summary>
        /// <param name="stringToCamel"></param>
        /// <returns></returns>
        public static string ToCamel(this string stringToCamel)
        {
            if (string.IsNullOrEmpty(stringToCamel))
            {
                return string.Empty;
            }
            string lowered = stringToCamel.ToLower();
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(lowered);
        }
        /// <summary>
        /// Replace partial string with option to match whole word
        /// </summary>
        /// <param name="input"></param>
        /// <param name="find"></param>
        /// <param name="replace"></param>
        /// <param name="matchWholeWord"></param>
        /// <returns></returns>
        public static string SafeReplace(this string input, string find, string replace, bool matchWholeWord)
        {
            var escapedFind = Regex.Escape(find);
            string textToFind = matchWholeWord ? string.Format(@"( |^){0}(\s|$)", escapedFind) : escapedFind;

            var newtext = Regex.Replace(input, textToFind, " " + replace, RegexOptions.Multiline) + " ";
            return newtext;
        }
        /// <summary>
        /// Replace partial string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string Replace(this string s, int index, int length, string replacement)
        {
            var builder = new StringBuilder();
            builder.Append(s.Substring(0, index));
            builder.Append(replacement);
            builder.Append(s.Substring(index + length));
            return builder.ToString();
        }
        /// <summary>
        /// Converts string to nullable int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int? ToNullableInt(this string value)
        {
            return int.TryParse(value, out var parsedInt) ? (int?)parsedInt : null;
        }
        /// <summary>
        /// Appends slash to the end of a string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AddSlash(this string value)
        {
            return value.EndsWith("/") ? value : $"{value}/";
        }
        /// <summary>
        /// Escapes curly braces inside a string
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string EscapeCurlyBraces(this string message)
        {
            return message?.Replace("{", "{{").Replace("}", "}}");
        }
        /// <summary>
        /// Splits the string at camel case and returns a string array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string[] SplitAtCamelCase(string value)
        {
            string strRegex = @"(?<=[a-z])([A-Z])|(?<=[A-Z])([A-Z][a-z])";
            Regex myRegex = new Regex(strRegex, RegexOptions.None);
            string strTargetstring = value;
            string strReplace = @" $1$2";

            return myRegex.Replace(strTargetstring, strReplace).Split(' ');
        }

        public static string SpaceAtCamelCase(string value)
        {
            string strRegex = @"(?<=[a-z])([A-Z])|(?<=[A-Z])([A-Z][a-z])";
            Regex myRegex = new Regex(strRegex, RegexOptions.None);
            string strTargetstring = value;
            string strReplace = @" $1$2";

            return myRegex.Replace(strTargetstring, strReplace);
        }
        public static string[] SplitAtSpace(string value)
        {
            return value.Split(' ');
        }
        public static string SplitAtSpace(string value, int indexToReturn)
        {
            return value.Split(' ')[indexToReturn];
        }
        public static string ConvertstringValueToValidXPath(string value)
        {
            return value.Replace("'", "&quot;");
        }
        public static string ReturnNumbersOnly(string value)
        {
            return Regex.Replace(value, @"[^\d]", "");
        }
        public static string ConvertstringPropertyToValidXPath(string value)
        {
            if (value == "Text")
            {
                value = "Name";
            }
            return value;
        }
    }
}
