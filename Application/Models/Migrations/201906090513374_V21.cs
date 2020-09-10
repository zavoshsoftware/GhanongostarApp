namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VipPackages", "ProductId", c => c.Guid(nullable: false));
            CreateIndex("dbo.VipPackages", "ProductId");
            AddForeignKey("dbo.VipPackages", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VipPackages", "ProductId", "dbo.Products");
            DropIndex("dbo.VipPackages", new[] { "ProductId" });
            DropColumn("dbo.VipPackages", "ProductId");
        }
    }
}
