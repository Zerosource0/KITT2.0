using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using KITTBackend.HelperClasses;
using KITTBackend.Models.ImplementedModels;
using KITTBackend.Services;
using KITTBackend.Services.ImplementedServices;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace KITTBackend.Identity
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private AppPermissionsService _appPermissionsService;

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            log.Info("GrantResourceOwnerCredentials - Context: " + context.ClientId + ", " + context.UserName);
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            UsersModel user = null;

            using (AuthRepository _repo = new AuthRepository())
            {
                user = await _repo.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }

            
            _appPermissionsService = new AppPermissionsService(new GenericService());

            List<AppPermissionsModel> appPermissionsModels = await _appPermissionsService.GetAppPermissionsByUsers(user.UsersID);
            UsersPermissionsCache cache = new UsersPermissionsCache();
            cache.Add(user.UsersID.ToString(), appPermissionsModels, DateTimeOffset.UtcNow.AddHours(1));
            

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            identity.AddClaim(new Claim("usersID", user.UsersID.ToString()));

            context.Validated(identity);

        }
    }

}