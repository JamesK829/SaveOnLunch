using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using James.Models.Common;
using System.Web.Mvc;
using System.Web.Routing;

namespace James.Models
{
    public class Authority:AuthorizeAttribute
    {
        public CAauthority authority { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            HttpSessionStateBase session = httpContext.Session;

            if (authority==CAauthority.normalCustomer)
            {
                if (session[CDictionary.SK_LOGINED_USER_ID] != null)
                    return true;
            }
            if (authority == CAauthority.admin)
            {
                if (session[CDictionary.SK_LOGINED_ADMIN_ID] != null)
                    return true;

            }
            if (authority == CAauthority.restaurant)
            {
                if (session[CDictionary.SK_LOGINED_RESTAURANT_ID] != null)
                    return true;
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            if (session[CDictionary.SK_LOGINED_USER_ID] == null) {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                
                    new { Controller = "Acc", Action = "Login" }
                ));
            }else if (session[CDictionary.SK_LOGINED_ADMIN_ID] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(

                    new { Controller = "Acc", Action = "Center" }
                ));
            }
            else if (session[CDictionary.SK_LOGINED_RESTAURANT_ID] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(

                    new { Controller = "Acc", Action = "Center" }
                ));
            }
        }

    }
}