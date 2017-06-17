
using AutoFacMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoFacMvc.Filters
{
    public class CustomActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 通过autofac注入ILogger服务
        /// </summary>
        public ILogger log { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            log.Log("OnActionExecuting");
        }
    }
}
