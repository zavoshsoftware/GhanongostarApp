namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V28 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Amount", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Amount", c => c.Decimal(storeType: "money"));
        }
    }
}
