namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V50 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmpClubVideoGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.EmpClubProducts", "EmpClubVideoGroupId", c => c.Guid());
            CreateIndex("dbo.EmpClubProducts", "EmpClubVideoGroupId");
            AddForeignKey("dbo.EmpClubProducts", "EmpClubVideoGroupId", "dbo.EmpClubVideoGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmpClubProducts", "EmpClubVideoGroupId", "dbo.EmpClubVideoGroups");
            DropIndex("dbo.EmpClubProducts", new[] { "EmpClubVideoGroupId" });
            DropColumn("dbo.EmpClubProducts", "EmpClubVideoGroupId");
            DropTable("dbo.EmpClubVideoGroups");
        }
    }
}
