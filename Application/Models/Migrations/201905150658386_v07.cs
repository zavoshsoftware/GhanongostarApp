namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v07 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ExpireNumber", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ExpireNumber", c => c.Int(nullable: false));
        }
    }
}
