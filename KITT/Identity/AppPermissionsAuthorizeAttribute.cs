using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using KITTBackend.HelperClasses;
using KITTBackend.Models.ImplementedModels;
using Thinktecture.IdentityModel.Extensions;


namespace KITTBackend.Identity
{   
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AppPermissionsAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public PermissionsEnum PermissionsName { get; set; }


        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
            var ctx = actionContext.Request.GetOwinContext();
            


            if (!principal.Identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }

            if (!principal.HasClaim(x => x.Type == "usersID"))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }

            var claim = principal.Claims.Last();
            UsersPermissionsCache cache = new UsersPermissionsCache();
            

            List<AppPermissionsModel> appPermissionsModels = (List<AppPermissionsModel>)cache.GetValue(claim.ToString().Split(' ').Last());
            if (appPermissionsModels == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }


            var dictionary = appPermissionsModels.ToDictionary(x => x.AppPermissionsID, y => y.AppPermissionsName);

            if (!dictionary.ContainsValue(PermissionsName.ToString()))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }


            //User is Authorized, complete execution
            return Task.FromResult<object>(null);

        }

        

    }
}