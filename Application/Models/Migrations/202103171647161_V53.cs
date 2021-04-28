namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V53 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "SitePassword", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "SitePassword");
        }
    }
}
