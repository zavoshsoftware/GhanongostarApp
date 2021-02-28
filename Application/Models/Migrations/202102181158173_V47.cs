namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V47 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmpClubQuestions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        Subject = c.String(),
                        Question = c.String(),
                        Response = c.String(),
                        ResponseDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            AddColumn("dbo.EmpClubProducts", "EmpClubProductGroupId", c => c.Guid(nullable: false));
            CreateIndex("dbo.EmpClubProducts", "EmpClubProductGroupId");
            AddForeignKey("dbo.EmpClubProducts", "EmpClubProductGroupId", "dbo.EmpClubProductGroups", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmpClubProducts", "EmpClubProductGroupId", "dbo.EmpClubProductGroups");
            DropForeignKey("dbo.EmpClubQuestions", "UserId", "dbo.Users");
            DropIndex("dbo.EmpClubProducts", new[] { "EmpClubProductGroupId" });
            DropIndex("dbo.EmpClubQuestions", new[] { "UserId" });
            DropColumn("dbo.EmpClubProducts", "EmpClubProductGroupId");
            DropTable("dbo.EmpClubQuestions");
        }
    }
}
