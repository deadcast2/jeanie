namespace jeanie.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateSettingsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true),
                    DailyReservationLimit = c.Int(),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.Settings");
        }
    }
}
