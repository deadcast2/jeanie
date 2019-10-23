using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(jeanie.Startup))]
namespace jeanie
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
