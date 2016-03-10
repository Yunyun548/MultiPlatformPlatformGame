using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MultiPlatfomPlatformGame.WebEditorASP.Startup))]
namespace MultiPlatfomPlatformGame.WebEditorASP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
