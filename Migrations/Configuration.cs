namespace jeanie.Migrations
{
    using jeanie.Lib;
    using jeanie.Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<JeanieContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(JeanieContext context)
        {
#if DEBUG
            Seeds.DefaultUser(ApplicationDbContext.Create());
#endif
            Seeds.DefaultSetting(context);
        }
    }
}
