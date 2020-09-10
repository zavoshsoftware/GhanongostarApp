namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V26 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Product_Id", "dbo.Products");
            DropIndex("dbo.Orders", new[] { "Product_Id" });
            AddColumn("dbo.Orders", "OrderTypeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Orders", "OrderTypeId");
            AddForeignKey("dbo.Orders", "OrderTypeId", "dbo.ProductTypes", "Id", cascadeDelete: false);
            DropColumn("dbo.Orders", "Product_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Product_Id", c => c.Guid());
            DropForeignKey("dbo.Orders", "OrderTypeId", "dbo.ProductTypes");
            DropIndex("dbo.Orders", new[] { "OrderTypeId" });
            DropColumn("dbo.Orders", "OrderTypeId");
            CreateIndex("dbo.Orders", "Product_Id");
            AddForeignKey("dbo.Orders", "Product_Id", "dbo.Products", "Id");
        }
    }
}
