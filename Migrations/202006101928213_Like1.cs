namespace ReviewArena.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Like1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Likes", "LikeAddedAt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Likes", "LikeAddedAt", c => c.DateTime(nullable: false));
        }
    }
}
