using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DotNetWebStudy.Startup))]
namespace DotNetWebStudy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
