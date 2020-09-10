namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v08 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourseDetails", "ThumbnailImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourseDetails", "ThumbnailImageUrl");
        }
    }
}
