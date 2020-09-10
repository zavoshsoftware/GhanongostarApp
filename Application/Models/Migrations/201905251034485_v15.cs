namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SupportRequests", "Code", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SupportRequests", "Code");
        }
    }
}
