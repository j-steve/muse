using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Muse.Models
{
    public class TvEpisode
    {
        [Key]
        public int id { get; set; }

        public string TVDB_ID { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }

        [Required]
        public int SeasonNumber { get; set; }

        [Required]
        public int EpisodeNumber { get; set; }

        public DateTime? FirstAired { get; set; }

        public string ImageUrl { get; set; }

    }
}