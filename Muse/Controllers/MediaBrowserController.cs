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
            ViewBag.Shows = GetTVShowList(); // new List<string> { "Starship", "Star Trek", "Star Wars" };//GetTVShowList();
            return View(); 
        }

        public ActionResult AddTvShow(string tvShowName)
        {
            string httpText = Common.GetHttpText("http://thetvdb.com/api/GetSeries.php?seriesname=" + Url.Encode(tvShowName));

            var shows = new List<Muse.Models.TvShow> { };
            foreach (string showData in Common.MultiSubstring(httpText, "<Series>", "</Series>"))
            {
                var show = new Muse.Models.TvShow();
                show.TVDB_ID = Common.Substring(showData, "<seriesid>", "</seriesid>");
                show.Name = Common.Substring(showData, "<SeriesName>", "</SeriesName>");
                show.FirstAired = Common.TryParseDateTime(Common.Substring(showData, "<FirstAired>", "</FirstAired>")) ?? new DateTime();
                show.Network = Common.Substring(showData, "<Network>", "</Network>");
                shows.Add(show);
            }

            return View(shows);

        }

        private HashSet<string> GetTVShowList()
        {
            string httpSource = Common.GetHttpText("http://en.wikipedia.org/wiki/List_of_television_programs_by_name");

            var shows = new HashSet<string> { };
            foreach (string showLine in Common.MultiSubstring(httpSource, "<li><i><a href=", "</a>"))
            {
                string showName = Common.Substring(showLine, ">");
                showName = showName.Replace("&amp;", "&");
                int trimAtIndex = showName.ToLowerInvariant().IndexOf("(tv series)");
                if (trimAtIndex != -1) { showName = showName.Substring(0, trimAtIndex).Trim() ;}
                shows.Add(Common.Substring(showLine, ">"));
            } 
            return shows;
        }
	}
}