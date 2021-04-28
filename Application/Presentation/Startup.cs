using Microsoft.Owin;
using Owin;

[assembly: OwinStartup("MyFirstProject2Config", typeof(Presentation.Startup))]
namespace Presentation

{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
