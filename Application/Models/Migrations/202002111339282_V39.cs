namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V39 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SiteBlogCategories",
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
                "dbo.SiteBlogs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Order = c.Int(nullable: false),
                        Body = c.String(),
                        ImageUrl = c.String(),
                        Summery = c.String(),
                        UrlParam = c.String(),
                        SiteBlogCategoryId = c.Guid(nullable: false),
                        OldUrlParam = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SiteBlogCategories", t => t.SiteBlogCategoryId, cascadeDelete: true)
                .Index(t => t.SiteBlogCategoryId);
            
            CreateTable(
                "dbo.SiteBlogImages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ImageAlt = c.String(),
                        SiteBlogId = c.Guid(nullable: false),
                        ImageUrl = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SiteBlogs", t => t.SiteBlogId, cascadeDelete: true)
                .Index(t => t.SiteBlogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SiteBlogImages", "SiteBlogId", "dbo.SiteBlogs");
            DropForeignKey("dbo.SiteBlogs", "SiteBlogCategoryId", "dbo.SiteBlogCategories");
            DropIndex("dbo.SiteBlogImages", new[] { "SiteBlogId" });
            DropIndex("dbo.SiteBlogs", new[] { "SiteBlogCategoryId" });
            DropTable("dbo.SiteBlogImages");
            DropTable("dbo.SiteBlogs");
            DropTable("dbo.SiteBlogCategories");
        }
    }
}
