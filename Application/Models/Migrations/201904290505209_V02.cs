namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V02 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.VersionHistories", "ApplicationTitle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VersionHistories", "ApplicationTitle", c => c.String());
        }
    }
}
