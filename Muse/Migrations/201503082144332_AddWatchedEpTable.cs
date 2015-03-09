namespace Muse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWatchedEpTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserTvEpisodes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TVDB_ID = c.String(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTvEpisodes", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserTvEpisodes", new[] { "User_Id" });
            DropTable("dbo.UserTvEpisodes");
        }
    }
}
