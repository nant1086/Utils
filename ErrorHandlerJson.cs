using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project;

namespace Project
{
    public class ErrorHandlerJson : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.Result = new JsonResult
            {
                Data = new Respuesta{ Error = true, Mensaje = filterContext.Exception.Message },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}