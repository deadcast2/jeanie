using jeanie.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Linq;

namespace jeanie.Lib
{
    public class Seeds
    {
        public static void DefaultUser(DbContext context)
        {
            const string email = "admin@example.com";
            var userStore = new UserStore<ApplicationUser>(context);
            if (userStore.Users.Where(e => e.Email == email).Count() == 0)
            {
                new ApplicationUserManager(new UserStore<ApplicationUser>(context)).Create(new ApplicationUser
                {
                    UserName = email,
                    Email = email
                }, "Temp123!");
            }
        }
    }
}
