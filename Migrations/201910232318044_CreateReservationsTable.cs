namespace jeanie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreateReservationsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reservations",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Name = c.String(nullable: false, maxLength: 100),
                    Grade = c.String(maxLength: 100),
                    PhoneNumber = c.String(maxLength: 60),
                    Notes = c.String(),
                    StartDate = c.DateTime(),
                    EndDate = c.DateTime(),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.Reservations");
        }
    }
}
