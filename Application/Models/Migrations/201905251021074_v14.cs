namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SupportRequests", "UserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.SupportRequests", "UserId");
            AddForeignKey("dbo.SupportRequests", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupportRequests", "UserId", "dbo.Users");
            DropIndex("dbo.SupportRequests", new[] { "UserId" });
            DropColumn("dbo.SupportRequests", "UserId");
        }
    }
}
