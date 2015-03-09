using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Muse.Models
{

    public class UserTvEpisode
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public string TVDB_ID { get; set; }
    }

    public class TvEpisode
    {
        [Key]
        public int ID { get; set; }

        public string TVDB_ID { get; set; }

        [Required]
        public TvShow TvShow { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int SeasonNumber { get; set; }

        [Required]
        public int EpisodeNumber { get; set; }

        public DateTime? FirstAired { get; set; }

        public string FirstAiredPrettified
        {
            get
            {
                if (!FirstAired.HasValue) { return null; }

                DateTime firstAired = FirstAired.Value;

                string result;

                int diff = Math.Abs(DateTime.Now.Date.Subtract(firstAired.Date).Days);

                if (diff == 0)
                {
                    result = "today";
                }
                else if (diff <= 7)
                {
                    result = firstAired.DayOfWeek.ToString().ToLower();
                }
                else if (diff < 180)
                {
                    result = firstAired.ToString("MMM dd");
                }
                else
                {
                    result = firstAired.ToShortDateString();
                }

                if (diff <= 7)
                {
                    if (firstAired.TimeOfDay == new DateTime().TimeOfDay && TvShow.AirTime.HasValue)
                    {
                        firstAired += TvShow.AirTime.Value.TimeOfDay;
                    }

                    if (firstAired.TimeOfDay != new DateTime().TimeOfDay)
                    {
                        result += " " + firstAired.ToShortTimeString();
                    }
                }

                return result;
            }
        }

        public string ImageUrl { get; set; }

        public bool Watched { get; set; }

        public TvEpisode (TvShow tvShow)
        {
            TvShow = tvShow;
        }


        private static string getTimeSpan(DateTime postDate)
        {
            string stringy = "";
            TimeSpan diff = DateTime.Now.Subtract(postDate);
            double years = Math.Floor(diff.TotalDays / 365);
            double weeks = Math.Floor(diff.TotalDays / 7);
            double days = diff.Days;
            double hours = diff.Hours + days * 24;
            double minutes = diff.Minutes + hours * 60;
            if (minutes <= 1)
            {
                stringy = "Just Now";
            }
            else if (years >= 1)
            {
                if (years >= 2)
                {
                    stringy = years.ToString() + " years ago";
                }
                else
                {
                    stringy = "1 year ago";
                }
            }
            else if (weeks >= 1)
            {
                if ((days - weeks * 7) > 0)
                {
                    if ((days - weeks * 7) > 1)
                    {
                        stringy = ", " + (days - weeks * 7).ToString() + " days";
                    }
                    else
                    {
                        stringy = ", " + (days - weeks * 7).ToString() + " day";
                    }
                }
                if (weeks >= 2)
                {
                    stringy = weeks.ToString() + " weeks" + stringy + " ago";
                }
                else
                {
                    stringy = "1 week" + stringy + " ago";
                }
            }
            else if (days >= 1)
            {
                if ((hours - days * 24) > 0)
                {
                    if ((hours - days * 24) > 1)
                    {
                        stringy = ", " + (hours - days * 24).ToString() + " hours";
                    }
                    else
                    {
                        stringy = ", " + (hours - days * 24).ToString() + " hour";
                    }
                }
                if (days >= 2)
                {
                    stringy = days.ToString() + " days" + stringy + " ago";
                }
                else
                {
                    stringy = "1 day" + stringy + " ago";
                }
            }
            else if (hours >= 1)
            {
                if ((minutes - hours * 60) > 0)
                {
                    if ((minutes - hours * 60) > 1)
                    {
                        stringy = ", " + (minutes - hours * 60).ToString() + " minutes";
                    }
                    else
                    {
                        stringy = ", " + (minutes - hours * 60).ToString() + " minute";
                    }
                }
                if (hours >= 2)
                {
                    stringy = hours.ToString() + " hours" + stringy + " ago";
                }
                else
                {
                    stringy = "1 hour" + stringy + " ago";
                }
            }
            else if (minutes > 1)
            {
                stringy = minutes.ToString() + " minutes ago";
            }
            return stringy;
        }

    }


}