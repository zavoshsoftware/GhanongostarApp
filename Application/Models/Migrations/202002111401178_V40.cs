namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V40 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SiteBlogCategories", "UrlParam", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SiteBlogCategories", "UrlParam");
        }
    }
}
