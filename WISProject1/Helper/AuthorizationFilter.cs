using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WISProject1.DAL;
using System.Web.Mvc;
using System.Web.Routing;

namespace WISProject1.Helper
{
 
        public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationContext filterContext)
            {
            WISProjectEntities db = new WISProjectEntities();
                string username = Convert.ToString(HttpContext.Current.Session["Username"]);
                string role = Convert.ToString(HttpContext.Current.Session["Role"]);
                string actionName = filterContext.ActionDescriptor.ActionName;
                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                string tag = controllerName + actionName;

                if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                    || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                {
                    // Don't check for authorization as AllowAnonymous filter is applied to the action or controller
                    return;
                }

                // Check for authorization
                if (System.Web.HttpContext.Current.Session["Username"] == null)
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
                if (username != null && username != "")
                {
                    bool isPermitted = false;
                    //string actionName = filterContext.ActionDescriptor.ActionName;
                    //string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                    //string tag = controllerName + actionName;
                    var viewPermission = db.Tbl_Roles.Where(x => x.Role == role && x.Tag == tag).SingleOrDefault();
                    if (viewPermission != null)
                    {
                        isPermitted = true;
                    }
                    if (isPermitted == false)
                    {
                        filterContext.Result = new RedirectToRouteResult(
                          new RouteValueDictionary
                            {
                                     { "controller", "Admin" },
                                     { "action", "AccessDenied" }
                            });
                    }
                }
            }
        }
    }