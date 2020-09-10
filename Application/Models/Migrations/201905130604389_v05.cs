namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v05 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ExpireNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "ExpireNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ExpireNumber");
            DropColumn("dbo.Orders", "ExpireNumber");
        }
    }
}
