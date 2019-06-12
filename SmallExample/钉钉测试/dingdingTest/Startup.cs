using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(dingdingTest.Startup))]
namespace dingdingTest
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
