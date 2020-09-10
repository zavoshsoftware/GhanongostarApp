namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "IsPrevious", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "IsPrevious", c => c.Int());
        }
    }
}
