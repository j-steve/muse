namespace Muse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakePropsRequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserTvShows", "TvShow_ID", "dbo.TvShows");
            DropForeignKey("dbo.UserTvShows", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserTvShows", new[] { "TvShow_ID" });
            DropIndex("dbo.UserTvShows", new[] { "User_Id" });
            AlterColumn("dbo.TvShows", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.UserTvShows", "TvShow_ID", c => c.Int(nullable: false));
            AlterColumn("dbo.UserTvShows", "User_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.UserTvShows", "TvShow_ID");
            CreateIndex("dbo.UserTvShows", "User_Id");
            AddForeignKey("dbo.UserTvShows", "TvShow_ID", "dbo.TvShows", "ID", cascadeDelete: true);
            AddForeignKey("dbo.UserTvShows", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTvShows", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserTvShows", "TvShow_ID", "dbo.TvShows");
            DropIndex("dbo.UserTvShows", new[] { "User_Id" });
            DropIndex("dbo.UserTvShows", new[] { "TvShow_ID" });
            AlterColumn("dbo.UserTvShows", "User_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.UserTvShows", "TvShow_ID", c => c.Int());
            AlterColumn("dbo.TvShows", "Name", c => c.String());
            CreateIndex("dbo.UserTvShows", "User_Id");
            CreateIndex("dbo.UserTvShows", "TvShow_ID");
            AddForeignKey("dbo.UserTvShows", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UserTvShows", "TvShow_ID", "dbo.TvShows", "ID");
        }
    }
}
