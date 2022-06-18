using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SYMVC.Startup))]
namespace SYMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
