using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperConsole
{
    public class Utilities
    {
        /// <summary>
        /// Trim the strings of a given string array
        /// </summary>
        /// <param name="trimParams"></param>
        /// <returns></returns>
        public static string[] TrimStringArray(string[] stringArray)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                stringArray[i] = stringArray[i].Trim();
            }
            return stringArray;
        }

        /// <summary>
        /// Convert an arry of strings to a string
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ConvertStringArrayToString(string[] array)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
                builder.Append(' ');
            }
            return builder.ToString();
        }
    }
}
