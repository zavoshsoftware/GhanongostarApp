namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V04 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "CityId", "dbo.Cities");
            DropIndex("dbo.Orders", new[] { "CityId" });
            AlterColumn("dbo.Orders", "CityId", c => c.Guid());
            CreateIndex("dbo.Orders", "CityId");
            AddForeignKey("dbo.Orders", "CityId", "dbo.Cities", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "CityId", "dbo.Cities");
            DropIndex("dbo.Orders", new[] { "CityId" });
            AlterColumn("dbo.Orders", "CityId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Orders", "CityId");
            AddForeignKey("dbo.Orders", "CityId", "dbo.Cities", "Id", cascadeDelete: true);
        }
    }
}
