namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V09 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlogCategories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Order = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Order = c.Int(nullable: false),
                        Body = c.String(),
                        ImageUrl = c.String(),
                        BlogCategoryId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BlogCategories", t => t.BlogCategoryId, cascadeDelete: true)
                .Index(t => t.BlogCategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blogs", "BlogCategoryId", "dbo.BlogCategories");
            DropIndex("dbo.Blogs", new[] { "BlogCategoryId" });
            DropTable("dbo.Blogs");
            DropTable("dbo.BlogCategories");
        }
    }
}
