namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V58 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SeminarImages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ImageAlt = c.String(),
                        SeminarId = c.Guid(nullable: false),
                        ImageUrl = c.String(),
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
            
            AlterColumn("dbo.Seminars", "ExecuteDate", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SeminarImages", "SeminarId", "dbo.Seminars");
            DropIndex("dbo.SeminarImages", new[] { "SeminarId" });
            AlterColumn("dbo.Seminars", "ExecuteDate", c => c.DateTime(nullable: false));
            DropTable("dbo.SeminarImages");
        }
    }
}
