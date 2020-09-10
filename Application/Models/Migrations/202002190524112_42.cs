namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _42 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Redirects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OldUrl = c.String(),
                        NewUrl = c.String(),
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
            DropTable("dbo.Redirects");
        }
    }
}
