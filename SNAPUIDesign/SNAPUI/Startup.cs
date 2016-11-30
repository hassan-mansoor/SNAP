using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SNAPUI.Startup))]
namespace SNAPUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
