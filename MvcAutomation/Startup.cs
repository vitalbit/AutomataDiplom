using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcAutomation.Startup))]
namespace MvcAutomation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
