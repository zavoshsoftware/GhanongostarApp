namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V38 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductGroups", "UrlParam", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductGroups", "UrlParam");
        }
    }
}
