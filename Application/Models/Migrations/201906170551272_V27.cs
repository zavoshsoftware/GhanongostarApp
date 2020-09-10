namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V27 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Point", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Point", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
