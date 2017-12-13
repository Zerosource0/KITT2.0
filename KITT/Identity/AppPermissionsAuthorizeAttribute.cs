using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using KITT.Models.DatabaseModels;
using KITTBackend.Identity;

namespace KITT.Identity
{   
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AppPermissionsAuthorizeAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public PermissionsEnum PermissionsName { get; set; }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            throw new NotImplementedException();

        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            
            var principal = filterContext.HttpContext.User as ClaimsPrincipal;
            var ctx = filterContext.HttpContext.GetOwinContext();

            if (!principal.Identity.IsAuthenticated)
            {
                //actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                filterContext.Result = new HttpUnauthorizedResult();
            }

            if (!principal.HasClaim(x => x.Type == "usersID"))
            {
                filterContext.Result = new HttpUnauthorizedResult();

            }

            var claim = principal.Claims.Last();
            UsersPermissionsCache cache = new UsersPermissionsCache();

            List<Permissions> permissions = (List<Permissions>)cache.GetValue(claim.ToString().Split(' ').Last());
            if (permissions == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }

            var dictionary = permissions.ToDictionary(x => x.Id, y => y.PermissionsName);

            if (!dictionary.ContainsValue(PermissionsName.ToString()))
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}