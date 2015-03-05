using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Muse.Controllers
{
    public class MediaBrowserController : Controller
    {
        //
        // GET: /Browser/
        public ActionResult Index()
        {
            ViewBag.Shows = GetTVShowList();
            return View(); 
        }

        private List<string> Shows;

        private List<string> GetTVShowList()
        {
            System.Net.WebRequest req = System.Net.HttpWebRequest.Create("http://en.wikipedia.org/wiki/List_of_television_programs_by_name"); 
            req.Method = "GET";
            
            string httpSource;
            using (var reader = new System.IO.StreamReader(req.GetResponse().GetResponseStream()))
            {
                httpSource = reader.ReadToEnd();
            }

            var shows = new List<string> { };
            foreach (string showLine in MultiSubstring(httpSource, "<li><i><a href=", "</a>"))
            {
                string showName = Substring(showLine, ">");
                showName = showName.Replace("&amp;", "&");
                int trimAtIndex = showName.ToLowerInvariant().IndexOf("(tv series)");
                if (trimAtIndex != -1) { showName = showName.Substring(0, trimAtIndex).Trim() ;}
                shows.Add(Substring(showLine, ">"));
            } 
            return shows;
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
                /*if (clusivity == Clusivity.Exclusive)*/ startIndex += startText.Length;
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