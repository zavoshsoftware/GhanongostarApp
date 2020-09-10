namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V41 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "DiscountAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Products", "IsInPromotion", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "IsInPromotion");
            DropColumn("dbo.Products", "DiscountAmount");
        }
    }
}
