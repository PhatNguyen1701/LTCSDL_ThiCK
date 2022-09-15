using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_1851010095_NguyenHongPhat_ASPNET.Startup))]
namespace _1851010095_NguyenHongPhat_ASPNET
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
