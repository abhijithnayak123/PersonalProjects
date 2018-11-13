using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MGI.Common.TransactionalLogging.Startup))]
namespace MGI.Common.TransactionalLogging
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
