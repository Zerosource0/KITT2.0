using System;
using KITT.Facade.Initialization;
using KITT.Identity;
using KITT.Models.DatabaseModels.BaseClass;
using KITTBackend.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Serilog;

[assembly: OwinStartup(typeof(KITT.OwinStartup))]
namespace KITT
{
    public class OwinStartup
    {

        public static void Configuration(IAppBuilder app)
        {
            Log.Logger = new LoggerConfiguration()
            .ReadFrom.AppSettings()
            .CreateLogger();

            Log.Logger.Information("Initialized Log");

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            IocConfig.Configure(app);

            ConfigureDatabaseSchema();
            
            //ConfigureOAuth(app);

        }

        public static void ConfigureDatabaseSchema()
        {
            DatabaseSchemaHandler schemaHandler = new DatabaseSchemaHandler();
            schemaHandler.CompareAndVerifyTablesInDatabase(typeof(DatabaseModel));
        }

        public void ConfigureOAuth(IAppBuilder app)
        {

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new AuthorizationServerProvider(),

            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}