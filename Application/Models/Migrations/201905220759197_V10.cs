namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "IsPrevious", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "IsPrevious");
        }
    }
}
