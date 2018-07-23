using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Splendor.Startup))]
namespace Splendor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
