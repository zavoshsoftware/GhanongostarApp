namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SupportRequests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TypeId = c.Guid(nullable: false),
                        Body = c.String(),
                        Status = c.Int(nullable: false),
                        Response = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SupportRequestTypes", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.TypeId);
            
            CreateTable(
                "dbo.SupportRequestTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Order = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "IsVip", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "Fullname", c => c.String());
            AddColumn("dbo.Orders", "Mobile", c => c.String());
            AddColumn("dbo.Orders", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupportRequests", "TypeId", "dbo.SupportRequestTypes");
            DropIndex("dbo.SupportRequests", new[] { "TypeId" });
            DropColumn("dbo.Orders", "Email");
            DropColumn("dbo.Orders", "Mobile");
            DropColumn("dbo.Orders", "Fullname");
            DropColumn("dbo.Users", "IsVip");
            DropTable("dbo.SupportRequestTypes");
            DropTable("dbo.SupportRequests");
        }
    }
}
