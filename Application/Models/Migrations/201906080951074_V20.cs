namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "IsVip", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "IsVip");
        }
    }
}
