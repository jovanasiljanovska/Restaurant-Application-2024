using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RestaurantApp.Startup))]
namespace RestaurantApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
