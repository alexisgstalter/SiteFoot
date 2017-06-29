using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SiteFoot.Startup))]
namespace SiteFoot
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
