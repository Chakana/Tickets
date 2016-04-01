using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Web_Testing.Startup))]
namespace Web_Testing
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
