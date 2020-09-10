namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V29 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "VideoThumbnail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "VideoThumbnail");
        }
    }
}
