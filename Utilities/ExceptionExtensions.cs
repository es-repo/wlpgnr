using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace WallpaperGenerator.Utilities
{
    public static class ExceptionExtensions
    {
        public static string ToDetailString(this Exception exception)
        {
            StringBuilder builder = new StringBuilder();
            WriteExceptionDetails(builder, exception, 0);
            return builder.ToString();
        }

        private static void WriteExceptionDetails(StringBuilder stringBuilder, Exception exception, int level)
        {
            string indent = new string(' ', level);

            if (level > 0)
            {
                stringBuilder.AppendLine(indent + "=== INNER EXCEPTION ===");
            }

            Dictionary<string, string> properties = new Dictionary<string, string>
            {
                {"Message", exception.Message},
                {"StackTrace", exception.StackTrace}
            };

            foreach (var prop in properties.Keys)
            {
                string val = properties[prop];
                if (val != null)
                {
                    stringBuilder.AppendFormat("{0}{1}: {2}{3}", indent, prop, val, Environment.NewLine);
                }
            }

            foreach (DictionaryEntry de in exception.Data)
            {
                stringBuilder.AppendFormat("{0} {1} = {2}{3}", indent, de.Key, de.Value, Environment.NewLine);
            }

            if (exception.InnerException != null)
            {
                WriteExceptionDetails(stringBuilder, exception.InnerException, ++level);
            }
        }
    }
}
