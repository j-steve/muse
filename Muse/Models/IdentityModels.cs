using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Muse.Models;

namespace Muse.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    { 

        public ApplicationDbContext()
            : base("DefaultConnection")
        { }

        public DbSet<TvShow> TvShows { get; set; }

        public DbSet<UserTvShow> UserTvShows { get; set; }

        public DbSet<UserTvEpisode> UserTvEpisodes { get; set; }
    }

}