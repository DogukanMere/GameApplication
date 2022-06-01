using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GameApplication.Startup))]
namespace GameApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
