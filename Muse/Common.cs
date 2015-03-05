using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Muse
{
    static internal class Common
    {

        public static DateTime? TryParseDateTime(string text)
        {
            DateTime returnVal;
            if (DateTime.TryParse(text, out returnVal)) return returnVal; else return null;
        }

        public static int? TryParseInt(string text)
        {
            int returnVal;
            if (int.TryParse(text, out returnVal)) return returnVal; else return null;
        }

        public static decimal? TryParseDecimal(string text)
        {
            decimal returnVal;
            if (decimal.TryParse(text, out returnVal)) return returnVal; else return null;
        }

        public static string GetHttpText(string url)
        {
            System.Net.WebRequest req = System.Net.HttpWebRequest.Create(url);
            req.Method = "GET";

            string httpText;
            using (var reader = new System.IO.StreamReader(req.GetResponse().GetResponseStream()))
            {
                httpText = reader.ReadToEnd();
            }
            return httpText;
        }


        public static List<string> MultiSubstring(string target, string startText, string endText,
                                     StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            if (target == null) throw new ArgumentNullException("target");

            List<string> resultList = new List<string>();

            int startIndex = target.IndexOf(startText, comparisonType) + startText.Length;
            int endIndex = target.IndexOf(endText, startIndex, comparisonType);
            while (startIndex >= startText.Length && endIndex != -1)
            {
                int substringLength = endIndex - startIndex;
                string result = target.Substring(startIndex, substringLength);
                resultList.Add(result);

                endIndex += endText.Length;
                startIndex = target.IndexOf(startText, endIndex, comparisonType) + startText.Length;
                endIndex = target.IndexOf(endText, startIndex, comparisonType);
            }

            return resultList;
        }

        public static string Substring(string target, string startText = null, string endText = null,
                                       StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        //Clusivity clusivity = Clusivity.Exclusive)
        {
            if (target == null) throw new ArgumentNullException("target");

            int startIndex = 0;
            if (startText != null)
            {
                startIndex = target.IndexOf(startText, comparisonType);
                if (startIndex == -1) return null;
                /*if (clusivity == Clusivity.Exclusive)*/
                startIndex += startText.Length;
            }

            string result = target.Substring(startIndex);

            if (endText != null)
            {
                int endIndex = result.IndexOf(endText, comparisonType);
                if (endIndex == -1) return null;
                //if (clusivity == Clusivity.Inclusive) endIndex += endText.Length;
                result = result.Substring(0, endIndex);
            }

            return result;
        }
    }
}