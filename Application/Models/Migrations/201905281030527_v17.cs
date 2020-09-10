namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v17 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "PaymentDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "PaymentDate", c => c.DateTime(nullable: false));
        }
    }
}
