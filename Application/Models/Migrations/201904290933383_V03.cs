namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V03 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.Int(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpireDate = c.DateTime(),
                        OrderFile = c.String(),
                        CityId = c.Guid(nullable: false),
                        Address = c.String(),
                        PostalCode = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.UserId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProvinceId = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        IsCenter = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Provinces", t => t.ProvinceId, cascadeDelete: true)
                .Index(t => t.ProvinceId);
            
            CreateTable(
                "dbo.Provinces",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.Int(nullable: false),
                        ProductTypeId = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 250),
                        IsFree = c.Boolean(nullable: false),
                        Point = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FileUrl = c.String(),
                        Amount = c.Decimal(storeType: "money"),
                        Body = c.String(),
                        ImageUrl = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductTypes", t => t.ProductTypeId, cascadeDelete: true)
                .Index(t => t.ProductTypeId);
            
            CreateTable(
                "dbo.CourseDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        SessionNumber = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        Summery = c.String(),
                        VideoUrl = c.String(),
                        Body = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(maxLength: 250),
                        Name = c.String(maxLength: 250),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserId", "dbo.Users");
            DropForeignKey("dbo.Products", "ProductTypeId", "dbo.ProductTypes");
            DropForeignKey("dbo.Orders", "ProductId", "dbo.Products");
            DropForeignKey("dbo.CourseDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Cities", "ProvinceId", "dbo.Provinces");
            DropForeignKey("dbo.Orders", "CityId", "dbo.Cities");
            DropIndex("dbo.CourseDetails", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "ProductTypeId" });
            DropIndex("dbo.Cities", new[] { "ProvinceId" });
            DropIndex("dbo.Orders", new[] { "CityId" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.Orders", new[] { "ProductId" });
            DropTable("dbo.ProductTypes");
            DropTable("dbo.CourseDetails");
            DropTable("dbo.Products");
            DropTable("dbo.Provinces");
            DropTable("dbo.Cities");
            DropTable("dbo.Orders");
        }
    }
}
