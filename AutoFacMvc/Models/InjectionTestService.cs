using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoFacMvc.Models
{
    public class InjectionTestService : IService
    {

        public string Test()
        {
            return "Success";
        }
    }
}