namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserVipPackages", "ExpiredDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserVipPackages", "ExpiredDate");
        }
    }
}
