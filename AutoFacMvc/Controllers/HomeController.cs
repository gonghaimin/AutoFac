using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoFacMvc.Models;
using Autofac;

using AutoFacMvc.Filters;

namespace AutoFacMvc.Controllers
{

    public class HomeController : Controller
    {
        private readonly InjectionTestService _testService;
        private readonly Func<MPlatform, IMessage> _factory;
        public HomeController(InjectionTestService testService, 
            Func<MPlatform, IMessage> factory)
        {
            _testService = testService;
             _factory = factory;
        }
        [CustomActionFilter]
        [CustomAuthorizeAttribute]
        public ActionResult Index()
        {
            ViewBag.TestValue = _testService.Test();
            ViewBag.retult = _factory(MPlatform.B平台).Say("测试");
            return View();
        }     
    }
}