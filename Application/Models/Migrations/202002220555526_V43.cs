namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V43 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IsSiteOrder", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "IsSiteOrder");
        }
    }
}
