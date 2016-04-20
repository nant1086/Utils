using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project;

namespace Project
{
    public class ValidJsonModel :ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var Respuesta = new Respuesta();

            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                Respuesta.Error = true;
                Respuesta.Mensaje =
                string.Join("<br>", filterContext.Controller.ViewData.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

                filterContext.Result = filterContext.Result = new JsonResult
                {
                    Data = Respuesta,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
                base.OnActionExecuting(filterContext);
        }
    }
}