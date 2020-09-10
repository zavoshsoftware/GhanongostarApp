namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Email", c => c.String());
            AddColumn("dbo.Users", "Rate", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Rate");
            DropColumn("dbo.Users", "Email");
        }
    }
}
