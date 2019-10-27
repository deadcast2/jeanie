namespace jeanie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddBlockedDatesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlockedDates",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"),
                    Date = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Date, unique: true);
        }

        public override void Down()
        {
            DropIndex("dbo.BlockedDates", new[] { "Date" });
            DropTable("dbo.BlockedDates");
        }
    }
}
