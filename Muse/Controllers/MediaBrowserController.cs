using Muse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Muse.Controllers
{
    [Authorize]
    public class MediaBrowserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        //
        // GET: /Browser/
        public ActionResult Index()
        {
            var userID = User.Identity.GetUserId();
            var shows = db.UserTvShows.Where(x => x.User.Id == userID); 

            return View(shows); 
        }

        public ActionResult GetTvShowList()
        {
            return Json(GetTVShowList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get a list of all TV show names to use in search textbox typeahead.
        /// </summary>
        private HashSet<string> GetTVShowList()
        {
            string httpSource = Common.GetHttpText("http://en.wikipedia.org/wiki/List_of_television_programs_by_name");

            var shows = new HashSet<string> { };
            foreach (string showLine in Common.MultiSubstring(httpSource, "<li><i><a href=", "</a>"))
            {
                string showName = Common.Substring(showLine, ">");
                showName = showName.Replace("&amp;", "&");
                int trimAtIndex = showName.ToLowerInvariant().IndexOf("(tv series)");
                if (trimAtIndex != -1) { showName = showName.Substring(0, trimAtIndex).Trim(); }
                shows.Add(Common.Substring(showLine, ">"));
            }
            return shows;
        }

        public ActionResult SearchTvShowToAdd(string tvShowName)
        {
            if (String.IsNullOrEmpty(tvShowName)) { return RedirectToAction("Index"); }
            
            string httpText = Common.GetHttpText("http://thetvdb.com/api/GetSeries.php?seriesname=" + Url.Encode(tvShowName));

            var shows = new List<Muse.Models.TvShow> { };
            foreach (string showData in Common.MultiSubstring(httpText, "<Series>", "</Series>"))
            {
                var show = new Muse.Models.TvShow();
                show.TVDB_ID = Common.Substring(showData, "<seriesid>", "</seriesid>");
                show.Name = Common.Substring(showData, "<SeriesName>", "</SeriesName>").Replace("&amp;", "&");
                show.FirstAired = Common.TryParseDateTime(Common.Substring(showData, "<FirstAired>", "</FirstAired>"));
                show.Network = Common.Substring(showData, "<Network>", "</Network>");
                shows.Add(show);
            }

            return View(shows);

        }

        public ActionResult SelectTvShowToAdd(string tvdbID)
        {
            if (String.IsNullOrEmpty(tvdbID)) { return RedirectToAction("Index"); }

            TvShow show = TvShow.GetByTvdbID(db, tvdbID); 
            if (show == null)
            {
                show = new TvShow();
                string httpText = Common.GetHttpText("http://thetvdb.com/api/9244EC31A575C4E3/series/" + Url.Encode(tvdbID));
                show.TVDB_ID = Common.Substring(httpText, "<id>", "</id>");
                show.Name = Common.Substring(httpText, "<SeriesName>", "</SeriesName>");
                show.FirstAired = Common.TryParseDateTime(Common.Substring(httpText, "<FirstAired>", "</FirstAired>"));
                show.AirTime = Common.TryParseDateTime(Common.Substring(httpText, "<Airs_Time>", "</Airs_Time>"));
                show.ImageUrl = Common.Substring(httpText, "<poster>", "</poster>") ?? Common.Substring(httpText, "<banner>", "</banner>") ?? Common.Substring(httpText, "<fanart>", "</fanart>");
                show.Network = Common.Substring(httpText, "<Network>", "</Network>");
            }

            var userTvShow = new UserTvShow() { User = db.Users.Find(User.Identity.GetUserId()), TvShow = show };
            db.UserTvShows.Add(userTvShow);
            db.SaveChanges(); 

            return RedirectToAction("Index");

        }

        public ActionResult TvShowEpisodes(UserTvShow show)
        {
            if (show.ID == 0) { return RedirectToAction("Index"); }

            show = db.UserTvShows.Find(show.ID);
            string httpText = Common.GetHttpText("http://thetvdb.com/api/9244EC31A575C4E3/series/" + Url.Encode(show.TvShow.TVDB_ID) + "/all");
            var episodes = new List<TvEpisode> { };

            var userID = User.Identity.GetUserId();
            IEnumerable<string> episodeDatas = Common.MultiSubstring(httpText, "<Episode>", "</Episode>");

            if (episodeDatas.Count() > 500)
            {
                episodeDatas = episodeDatas.Skip(episodeDatas.Count() - 500);
            }

            foreach (string episodeData in episodeDatas)
            {
                var episode = new TvEpisode(show.TvShow);
                episode.TVDB_ID = Common.Substring(episodeData, "<id>", "</id>");
                episode.Name = Common.Substring(episodeData, "<EpisodeName>", "</EpisodeName>");
                episode.Description = Common.Substring(episodeData, "<Overview>", "</Overview>");
                episode.SeasonNumber = Common.TryParseInt(Common.Substring(episodeData, "<SeasonNumber>", "</SeasonNumber>")) ?? 0;
                episode.EpisodeNumber = Common.TryParseInt(Common.Substring(episodeData, "<EpisodeNumber>", "</EpisodeNumber>")) ?? 0;
                episode.FirstAired = Common.TryParseDateTime(Common.Substring(episodeData, "<FirstAired>", "</FirstAired>"));
                string imageUrl = Common.Substring(episodeData, "<filename>", "</filename>");
                if (!string.IsNullOrEmpty(imageUrl)) { episode.ImageUrl = "http://thetvdb.com/banners/" + imageUrl; }
                episode.Watched = db.UserTvEpisodes.Where(x => x.User.Id == userID && x.TVDB_ID == episode.TVDB_ID).Count() > 0;

                episodes.Add(episode);
            }

            return View(episodes);
        }


        public ActionResult ToggleWatched()
        {
            string[] episodeTvdbIDs = Request.Form["episodeTvdbIds[]"].Split(',');
            foreach (string episodeTvdbID in episodeTvdbIDs)
            {
                var userID = User.Identity.GetUserId();
                var episode = db.UserTvEpisodes.Where(x => x.User.Id == userID && x.TVDB_ID == episodeTvdbID).SingleOrDefault();
                if (episode == null)
                {
                    episode = new UserTvEpisode { User = db.Users.Find(User.Identity.GetUserId()), TVDB_ID = episodeTvdbID };
                    db.UserTvEpisodes.Add(episode);
                }
                else
                {
                    db.UserTvEpisodes.Remove(episode);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

	}
}