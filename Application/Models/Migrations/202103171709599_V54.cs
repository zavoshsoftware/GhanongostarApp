namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V54 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "AppPassword", c => c.String(maxLength: 150));
            DropColumn("dbo.Users", "SitePassword");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "SitePassword", c => c.String(maxLength: 150));
            DropColumn("dbo.Users", "AppPassword");
        }
    }
}
