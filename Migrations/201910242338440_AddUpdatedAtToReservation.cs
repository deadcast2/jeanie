namespace jeanie.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddUpdatedAtToReservation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "UpdatedAt", c => c.DateTime());
        }

        public override void Down()
        {
            DropColumn("dbo.Reservations", "UpdatedAt");
        }
    }
}
