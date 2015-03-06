namespace Muse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EpsButNo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TvShows", "ImageUrl", c => c.String());
            AlterColumn("dbo.TvShows", "FirstAired", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TvShows", "FirstAired", c => c.DateTime(nullable: false));
            DropColumn("dbo.TvShows", "ImageUrl");
        }
    }
}
