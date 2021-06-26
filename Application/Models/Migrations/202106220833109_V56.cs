namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V56 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Seminars",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Body = c.String(),
                        ExecuteDate = c.DateTime(nullable: false),
                        IsNew = c.Boolean(nullable: false),
                        Place = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SeminarTeachers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        ImageUrl = c.String(),
                        SeminarId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Seminars", t => t.SeminarId, cascadeDelete: true)
                .Index(t => t.SeminarId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SeminarTeachers", "SeminarId", "dbo.Seminars");
            DropIndex("dbo.SeminarTeachers", new[] { "SeminarId" });
            DropTable("dbo.SeminarTeachers");
            DropTable("dbo.Seminars");
        }
    }
}
