namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V49 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConsultantRequestForms",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Company = c.String(),
                        ActionType = c.String(),
                        EmployeeQuantity = c.String(),
                        ContactNumber = c.String(),
                        Message = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ConsultantRequestForms");
        }
    }
}
