namespace jeanie.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ChangeMaxLengthOfNotes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reservations", "Notes", c => c.String(maxLength: 1000));
        }

        public override void Down()
        {
            AlterColumn("dbo.Reservations", "Notes", c => c.String());
        }
    }
}
