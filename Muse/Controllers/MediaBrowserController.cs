using Muse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Muse.Controllers
{
    public class MediaBrowserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: /Browser/
        public ActionResult Index()
        {
            ViewBag.Shows = new List<string> { "Starship", "Star Trek", "Star Wars" };//GetTVShowList();
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

        public ActionResult SelectTvShow(string tvdbID)
        {
            TvShow show = TvShow.GetByTvdbID(db, tvdbID);
            if (show == null)
            {
                show = new TvShow();
                string httpText = Common.GetHttpText("http://thetvdb.com/api/9244EC31A575C4E3/series/" + Url.Encode(tvdbID));
                show.TVDB_ID = Common.Substring(httpText, "<id>", "</id>");
                show.Name = Common.Substring(httpText, "<SeriesName>", "</SeriesName>");
                show.FirstAired = Common.TryParseDateTime(Common.Substring(httpText, "<FirstAired>", "</FirstAired>")) ?? new DateTime();
                show.Network = Common.Substring(httpText, "<Network>", "</Network>");
            }

            var userTvShow = new UserTvShow() { User = db.Users.Find(User.Identity.GetUserId()), TvShow = show };
            db.UserTvShows.Add(userTvShow);
            db.SaveChanges(); 

            return RedirectToAction("Index");

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