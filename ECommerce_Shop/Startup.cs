using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ECommerce_Shop.Startup))]
namespace ECommerce_Shop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}