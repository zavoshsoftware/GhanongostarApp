namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V06 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionConversations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        Subject = c.String(),
                        Body = c.String(),
                        StatusCode = c.Int(nullable: false),
                        UserId = c.Guid(nullable: false),
                        ParentId = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionConversations", t => t.ParentId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ParentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionConversations", "UserId", "dbo.Users");
            DropForeignKey("dbo.QuestionConversations", "ParentId", "dbo.QuestionConversations");
            DropIndex("dbo.QuestionConversations", new[] { "ParentId" });
            DropIndex("dbo.QuestionConversations", new[] { "UserId" });
            DropTable("dbo.QuestionConversations");
        }
    }
}
