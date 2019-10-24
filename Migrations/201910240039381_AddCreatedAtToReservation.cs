namespace jeanie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreatedAtToReservation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "CreatedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "CreatedAt");
        }
    }
}
