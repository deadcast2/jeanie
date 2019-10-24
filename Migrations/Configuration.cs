namespace jeanie.Migrations
{
    using jeanie.Lib;
    using jeanie.Models;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<jeanie.Lib.JeanieContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(jeanie.Lib.JeanieContext context)
        {
            Seeds.DefaultUser(ApplicationDbContext.Create());
        }
    }
}
