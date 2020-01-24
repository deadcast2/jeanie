namespace jeanie.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddStartAndEndDateToBlockedDatesTable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BlockedDates", new[] { "Date" });
            AddColumn("dbo.BlockedDates", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.BlockedDates", "EndDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.BlockedDates", new[] { "StartDate", "EndDate" }, unique: true);
            DropColumn("dbo.BlockedDates", "Date");
        }

        public override void Down()
        {
            AddColumn("dbo.BlockedDates", "Date", c => c.DateTime(nullable: false));
            DropIndex("dbo.BlockedDates", new[] { "StartDate", "EndDate" });
            DropColumn("dbo.BlockedDates", "EndDate");
            DropColumn("dbo.BlockedDates", "StartDate");
            CreateIndex("dbo.BlockedDates", "Date", unique: true);
        }
    }
}
