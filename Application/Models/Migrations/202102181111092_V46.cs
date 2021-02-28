namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V46 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmpClubProductGroups",
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
            
            CreateTable(
                "dbo.EmpClubProducts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        ImageUrl = c.String(),
                        FileUrl = c.String(),
                        Body = c.String(),
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
            DropTable("dbo.EmpClubProducts");
            DropTable("dbo.EmpClubProductGroups");
        }
    }
}
