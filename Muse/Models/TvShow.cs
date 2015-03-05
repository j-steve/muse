using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Muse.Models
{
    public class TvShow
    {
        [Key]
        [DataType(DataType.Text)]
        [Display(Name = "TVDB ID")]
        public string TVDB_ID {get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [DataType(DataType.Date)] 
        [Display(Name = "First Aired")]
        public DateTime FirstAired { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Network")]
        public string Network { get; set; }


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