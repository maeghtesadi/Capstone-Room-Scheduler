using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CapstoneRoomScheduler.Startup))]
namespace CapstoneRoomScheduler
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
