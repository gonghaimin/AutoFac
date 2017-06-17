using AutoFacMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoFacMvc.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 通过autofac注入ILogger服务
        /// </summary>
        public ILogger log { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            log.Log("AuthorizeCore");
            return true;
        }
    }
}