using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BPTrackers.Startup))]
namespace BPTrackers
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
