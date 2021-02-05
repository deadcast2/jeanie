namespace jeanie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailTemplateToSettingsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "EmailTemplate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "EmailTemplate");
        }
    }
}
