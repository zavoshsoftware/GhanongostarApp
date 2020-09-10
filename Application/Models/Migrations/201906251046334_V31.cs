namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V31 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DiscountCodes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(nullable: false, maxLength: 10),
                        ExpireDate = c.DateTime(nullable: false),
                        IsPercent = c.Boolean(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsMultiUsing = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderDiscounts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderId = c.Guid(nullable: false),
                        DiscountCodeId = c.Guid(nullable: false),
                        IsUse = c.Boolean(nullable: false),
                        UsingDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiscountCodes", t => t.DiscountCodeId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.DiscountCodeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDiscounts", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderDiscounts", "DiscountCodeId", "dbo.DiscountCodes");
            DropIndex("dbo.OrderDiscounts", new[] { "DiscountCodeId" });
            DropIndex("dbo.OrderDiscounts", new[] { "OrderId" });
            DropTable("dbo.OrderDiscounts");
            DropTable("dbo.DiscountCodes");
        }
    }
}
