using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using KITT.Identity;
using KITT.Models.DatabaseModels;
using KITTBackend.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace KITT.Identity
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        //private AppPermissionsService _appPermissionsService;

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //log.Info("GrantResourceOwnerCredentials - Context: " + context.ClientId + ", " + context.UserName);
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            User user = null;

            using (AuthRepository _repo = new AuthRepository())
            {
                user = await _repo.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }

            
            //_appPermissionsService = new AppPermissionsService(new GenericService());

            //List<Permissions> appPermissionsModels = await _appPermissionsService.GetAppPermissionsByUsers(user.UsersId);
            //UsersPermissionsCache cache = new UsersPermissionsCache();
            //cache.Add(user.UsersId.ToString(), appPermissionsModels, DateTimeOffset.UtcNow.AddHours(1));
            

            //var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            //identity.AddClaim(new Claim("sub", context.UserName));
            //identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            //identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            //identity.AddClaim(new Claim("usersId", user.UsersId.ToString()));

            //context.Validated(identity);

        }
    }

}