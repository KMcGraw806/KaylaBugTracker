using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KaylaBugTracker.Startup))]
namespace KaylaBugTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
