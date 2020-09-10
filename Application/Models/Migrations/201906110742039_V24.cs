namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V24 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VipPackageFeatures",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        VipPackageId = c.Guid(nullable: false),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VipPackages", t => t.VipPackageId, cascadeDelete: true)
                .Index(t => t.VipPackageId);
            
            CreateTable(
                "dbo.Texts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
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
            DropForeignKey("dbo.VipPackageFeatures", "VipPackageId", "dbo.VipPackages");
            DropIndex("dbo.VipPackageFeatures", new[] { "VipPackageId" });
            DropTable("dbo.Texts");
            DropTable("dbo.VipPackageFeatures");
        }
    }
}
