namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V37 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PageCounts", "EntityId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PageCounts", "EntityId");
        }
    }
}
