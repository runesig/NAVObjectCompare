﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVObjectCompare
{
    public class ObjectHelper
    {
        public static string GetObjectName(string line)
        {
            string[] parts = line.Split(' ');

            if (parts.Length < 3)
                throw new Exception(string.Format("Could not split string {0} into several parts. Something is incorrect with 'OBJECT' line.", line));
        
            string firstPartOfLine = string.Format("{0} {1} {2} ", parts[0], parts[1], parts[2]);
            return line.Replace(firstPartOfLine, string.Empty);
        }

        public static string GetVersionList(string line, string versionListPart)
        {
            line = line.Replace(versionListPart, string.Empty);

            if (line.Length > 1)
            {
                line = line.Substring(1);
                line = line.Remove(line.Length - 1);
            }
            return line;
        }
        public static DateTime GetDateTime(string stringValue)
        {
            stringValue = RemoveIllChar(stringValue);

            DateTime date;
            if (!DateTime.TryParseExact(stringValue,
                                   "dd.MM.yy hh:mm:ss",
                                   CultureInfo.InvariantCulture,
                                   DateTimeStyles.None,
                                   out date))
                throw new Exception(string.Format("{0} could not be converted to 'DateTime'", stringValue));

            //if(!DateTime.TryParse(stringValue, out date))
            //    throw new Exception(string.Format("{0} could not be converted to 'DateTime'", stringValue));

            return date;
        }

        public static int GetInt(string stringValue)
        {
            stringValue = RemoveIllChar(stringValue);

            int intValue = 0;
            if (!int.TryParse(stringValue, out intValue))
                throw new Exception(string.Format("{0} could not be converted to 'Int'", stringValue));

            return intValue;
        }

        public static bool GetBool(string stringValue)
        {
            stringValue = RemoveIllChar(stringValue);

            switch (stringValue)
            {
                case "Yes":
                    return true;
                case "False":
                    return false;
            }

            bool boolValue = false;
            if (!bool.TryParse(stringValue, out boolValue))
                throw new Exception(string.Format("{0} could not be converted to 'Bool'", stringValue));

            return boolValue;
        }

        public static string RemoveIllChar(string stringPart)
        {
            stringPart = stringPart.Trim();
            stringPart = stringPart.Replace(";", string.Empty);

            return stringPart;
        }
    }
}
