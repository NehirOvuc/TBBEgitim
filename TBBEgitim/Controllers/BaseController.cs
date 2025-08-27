using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TBBEgitim.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["User"] == null &&
                !(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Account"
                  && filterContext.ActionDescriptor.ActionName == "Login"))
            {
                filterContext.Result = RedirectToAction("Login", "Account");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
