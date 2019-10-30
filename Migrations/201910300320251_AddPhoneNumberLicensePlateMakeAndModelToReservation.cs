namespace jeanie.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddPhoneNumberLicensePlateMakeAndModelToReservation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "PhoneNumber", c => c.String(maxLength: 24));
            AddColumn("dbo.Reservations", "LicensePlateNumber", c => c.String(maxLength: 8));
            AddColumn("dbo.Reservations", "MakeAndModel", c => c.String(maxLength: 100));
        }

        public override void Down()
        {
            DropColumn("dbo.Reservations", "MakeAndModel");
            DropColumn("dbo.Reservations", "LicensePlateNumber");
            DropColumn("dbo.Reservations", "PhoneNumber");
        }
    }
}
