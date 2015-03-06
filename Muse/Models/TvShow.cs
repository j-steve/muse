using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Muse.Models
{
    public class UserTvShow
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public TvShow TvShow { get; set; }

    }

    public class TvShow
    {
        public static TvShow GetByTvdbID(ApplicationDbContext db, string tvdbID)
        { 
            return db.TvShows.Where(x => x.TVDB_ID == tvdbID).SingleOrDefault();
        }

        [Key]
        public int ID { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string TVDB_ID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [DataType(DataType.Date)] 
        [Display(Name = "First Aired")]
        public DateTime? FirstAired { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Network")]
        public string Network { get; set; }

        public string ImageUrl { get; set; }

        //public List<TvEpisode> Episodes { get; set; }

        public TvShow () { }

        public TvShow (string tvdbID, string name, DateTime firstAired = new DateTime(), string network = null)
        {
            TVDB_ID = tvdbID;
            Name = name;
            FirstAired = firstAired; 
            Network = network;
        }

    }
}