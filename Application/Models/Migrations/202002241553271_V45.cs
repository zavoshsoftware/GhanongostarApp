namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V45 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "IsInHome", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "IsInHome");
        }
    }
}
