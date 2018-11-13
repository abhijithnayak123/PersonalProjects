using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web
{    
    public class CustomHandleErrorAttribute : ActionFilterAttribute
    {
        public string MasterName { get; set; }
        public string ViewName { get; set; }
        public string ModelType { get; set; }
        public string ResultType {get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }

            public CustomHandleErrorAttribute()
            {
            }

            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var InputParam = filterContext.ActionParameters.FirstOrDefault();

                filterContext.Controller.ViewData.Add("ViewName", ViewName);
                filterContext.Controller.ViewData.Add("MasterName", MasterName);
                filterContext.Controller.ViewData.Add("ModelType", ModelType);
                filterContext.Controller.ViewData.Add("ResultType", ResultType);
                filterContext.Controller.ViewData.Add("ActionName", ActionName);
                filterContext.Controller.ViewData.Add("ControllerName", ControllerName);

                if (InputParam.Key != null)
                {
                    filterContext.Controller.ViewData.Add("ParamType", InputParam.Key);
                    filterContext.Controller.ViewData.Add("ParamValue", InputParam.Value);
                }

                base.OnActionExecuting(filterContext);
            }           
        }
    }