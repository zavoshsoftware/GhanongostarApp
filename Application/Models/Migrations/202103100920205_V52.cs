namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V52 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FormInstagramLives", "IsPaid", c => c.Boolean(nullable: false));
            AddColumn("dbo.FormInstagramLives", "OrderCode", c => c.String());
            AddColumn("dbo.FormInstagramLives", "SaleRefrenceId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FormInstagramLives", "SaleRefrenceId");
            DropColumn("dbo.FormInstagramLives", "OrderCode");
            DropColumn("dbo.FormInstagramLives", "IsPaid");
        }
    }
}
