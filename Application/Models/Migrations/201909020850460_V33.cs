namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V33 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Code = c.Int(nullable: false),
                        ImageUrl = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Products", "ProductGroupId", c => c.Guid());
            CreateIndex("dbo.Products", "ProductGroupId");
            AddForeignKey("dbo.Products", "ProductGroupId", "dbo.ProductGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "ProductGroupId", "dbo.ProductGroups");
            DropIndex("dbo.Products", new[] { "ProductGroupId" });
            DropColumn("dbo.Products", "ProductGroupId");
            DropTable("dbo.ProductGroups");
        }
    }
}
