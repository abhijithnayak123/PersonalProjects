using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TEST.Startup))]
namespace TEST
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
