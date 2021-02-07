namespace jeanie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailTemplateSubjectToSettingsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "EmailTemplateSubject", c => c.String(maxLength: 255));
            AddColumn("dbo.Settings", "EmailTemplateBody", c => c.String());
            DropColumn("dbo.Settings", "EmailTemplate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Settings", "EmailTemplate", c => c.String());
            DropColumn("dbo.Settings", "EmailTemplateBody");
            DropColumn("dbo.Settings", "EmailTemplateSubject");
        }
    }
}
