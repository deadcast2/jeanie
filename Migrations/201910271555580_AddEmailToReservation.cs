namespace jeanie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailToReservation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "Email", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "Email");
        }
    }
}
