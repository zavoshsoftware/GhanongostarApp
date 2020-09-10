namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V19 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserVipPackages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        VipPackegeId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                        VipPackage_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.VipPackages", t => t.VipPackage_Id)
                .Index(t => t.UserId)
                .Index(t => t.VipPackage_Id);
            
            CreateTable(
                "dbo.VipPackages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Duration = c.Int(nullable: false),
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
            DropForeignKey("dbo.UserVipPackages", "VipPackage_Id", "dbo.VipPackages");
            DropForeignKey("dbo.UserVipPackages", "UserId", "dbo.Users");
            DropIndex("dbo.UserVipPackages", new[] { "VipPackage_Id" });
            DropIndex("dbo.UserVipPackages", new[] { "UserId" });
            DropTable("dbo.VipPackages");
            DropTable("dbo.UserVipPackages");
        }
    }
}
