namespace jeanie.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RemovePhoneAndLicenseInfo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Reservations", "PhoneNumber");
            DropColumn("dbo.Reservations", "LicensePlateNumber");
            DropColumn("dbo.Reservations", "MakeAndModel");
        }

        public override void Down()
        {
            AddColumn("dbo.Reservations", "MakeAndModel", c => c.String(maxLength: 100));
            AddColumn("dbo.Reservations", "LicensePlateNumber", c => c.String(maxLength: 8));
            AddColumn("dbo.Reservations", "PhoneNumber", c => c.String(maxLength: 24));
        }
    }
}
