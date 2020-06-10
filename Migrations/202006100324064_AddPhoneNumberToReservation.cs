namespace jeanie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhoneNumberToReservation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "PhoneNumber", c => c.String(maxLength: 24));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "PhoneNumber");
        }
    }
}
