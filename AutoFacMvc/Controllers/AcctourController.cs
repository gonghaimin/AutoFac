using Autofac;
using Autofac.Integration.Mvc;
using AutoFacMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoFacMvc.Controllers
{
    public class AcctourController : Controller
    {
        // GET: Acctour
        private readonly MyService _myservice;
        public Repository<User> db { get; set; }
        public AcctourController(MyService myservice)
        {
            
            _myservice = myservice;
        }

        public ActionResult Index()
        {
            this.HttpContext.Items["user"] = 5;
            var _myservice2 = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<MyService>();
            ViewBag.Message = _myservice.Test() + "|" + _myservice2.Test();
            var test = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<MyService>();
            ViewBag.Message += "/" + test.Test();
            return View();
        }
  
    }
}