namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V25 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "ProductId", "dbo.Products");
            DropIndex("dbo.Orders", new[] { "ProductId" });
            RenameColumn(table: "dbo.Orders", name: "ProductId", newName: "Product_Id");
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderFile = c.String(),
                        Fullname = c.String(),
                        Quantity = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RawAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderId = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            AlterColumn("dbo.Orders", "Product_Id", c => c.Guid());
            CreateIndex("dbo.Orders", "Product_Id");
            AddForeignKey("dbo.Orders", "Product_Id", "dbo.Products", "Id");
            DropColumn("dbo.Orders", "OrderFile");
            DropColumn("dbo.Orders", "Fullname");
            DropColumn("dbo.Orders", "Mobile");
            DropColumn("dbo.Orders", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Email", c => c.String());
            AddColumn("dbo.Orders", "Mobile", c => c.String());
            AddColumn("dbo.Orders", "Fullname", c => c.String());
            AddColumn("dbo.Orders", "OrderFile", c => c.String());
            DropForeignKey("dbo.Orders", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropIndex("dbo.OrderDetails", new[] { "ProductId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "Product_Id" });
            AlterColumn("dbo.Orders", "Product_Id", c => c.Guid(nullable: false));
            DropTable("dbo.OrderDetails");
            RenameColumn(table: "dbo.Orders", name: "Product_Id", newName: "ProductId");
            CreateIndex("dbo.Orders", "ProductId");
            AddForeignKey("dbo.Orders", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
        }
    }
}
