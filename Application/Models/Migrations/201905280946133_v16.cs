namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v16 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ZarinpallAuthorities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Authority = c.String(),
                        OrderId = c.Guid(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            AddColumn("dbo.Orders", "IsPaid", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "PaymentDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ZarinpallAuthorities", "OrderId", "dbo.Orders");
            DropIndex("dbo.ZarinpallAuthorities", new[] { "OrderId" });
            DropColumn("dbo.Orders", "PaymentDate");
            DropColumn("dbo.Orders", "IsPaid");
            DropTable("dbo.ZarinpallAuthorities");
        }
    }
}
