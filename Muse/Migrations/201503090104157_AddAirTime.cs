namespace Muse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAirTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TvShows", "AirTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TvShows", "AirTime");
        }
    }
}
