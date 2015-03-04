using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Muse.Startup))]
namespace Muse
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
