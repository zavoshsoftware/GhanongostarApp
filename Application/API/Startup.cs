using API.Provider;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(API.Startup))]
namespace API
{
    public class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {

            var OAuthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(525600),
                Provider = new SimpleAuthorizationServerProvider()
            };

            app.UseOAuthBearerTokens(OAuthOptions);
            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);

          //  app.MapSignalR();

            //new NotificationService().Start();

        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        //    services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        //}

    }
}