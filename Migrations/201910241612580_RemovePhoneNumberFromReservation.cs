namespace jeanie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePhoneNumberFromReservation : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Reservations", "PhoneNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservations", "PhoneNumber", c => c.String(maxLength: 60));
        }
    }
}
