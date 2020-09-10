namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V32 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "TotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Orders", "DiscountAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "DiscountAmount");
            DropColumn("dbo.Orders", "TotalAmount");
        }
    }
}
