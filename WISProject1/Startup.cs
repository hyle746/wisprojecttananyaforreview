using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WISProject1.Startup))]
namespace WISProject1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
