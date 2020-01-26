namespace jeanie.Migrations
{
    using jeanie.Lib;
    using jeanie.Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<jeanie.Lib.JeanieContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(jeanie.Lib.JeanieContext context)
        {
#if DEBUG
            Seeds.DefaultUser(ApplicationDbContext.Create());
#endif
        }
    }
}
