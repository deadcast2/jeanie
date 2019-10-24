namespace jeanie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddComputedIdentityToReservation : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Reservations");
            AlterColumn("dbo.Reservations", "Id", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AddPrimaryKey("dbo.Reservations", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Reservations");
            AlterColumn("dbo.Reservations", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Reservations", "Id");
        }
    }
}
