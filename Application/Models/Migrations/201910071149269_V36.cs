namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V36 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PageCounts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PageId = c.Guid(nullable: false),
                        Count = c.Int(nullable: false),
                        VisitDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pages", t => t.PageId, cascadeDelete: true)
                .Index(t => t.PageId);
            
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 100),
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
            DropForeignKey("dbo.PageCounts", "PageId", "dbo.Pages");
            DropIndex("dbo.PageCounts", new[] { "PageId" });
            DropTable("dbo.Pages");
            DropTable("dbo.PageCounts");
        }
    }
}
