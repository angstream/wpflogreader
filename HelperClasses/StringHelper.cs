using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogReader.HelperClasses
{
    public static class StringHelper
    {

        public static bool ContainsIgnoreCase(this string source, string toCheck)
        {
            return source.Contains(toCheck, StringComparison.OrdinalIgnoreCase);
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static string ListToString(this List<string> list)
        {
            StringBuilder builder = new StringBuilder();            
            list.ForEach(line => builder.AppendLine(line));
            return builder.ToString();
        }
    }
}
