namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v13 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.SupportRequests", name: "TypeId", newName: "SupportRequestTypeId");
            RenameIndex(table: "dbo.SupportRequests", name: "IX_TypeId", newName: "IX_SupportRequestTypeId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.SupportRequests", name: "IX_SupportRequestTypeId", newName: "IX_TypeId");
            RenameColumn(table: "dbo.SupportRequests", name: "SupportRequestTypeId", newName: "TypeId");
        }
    }
}
