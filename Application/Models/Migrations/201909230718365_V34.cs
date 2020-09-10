namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V34 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductDiscounts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DiscountCodeId = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiscountCodes", t => t.DiscountCodeId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.DiscountCodeId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductDiscounts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductDiscounts", "DiscountCodeId", "dbo.DiscountCodes");
            DropIndex("dbo.ProductDiscounts", new[] { "ProductId" });
            DropIndex("dbo.ProductDiscounts", new[] { "DiscountCodeId" });
            DropTable("dbo.ProductDiscounts");
        }
    }
}
