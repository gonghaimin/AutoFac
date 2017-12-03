using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoFacMvc.Models
{
    public class MyService
    {
        public string Test()
        {
            return "Test"+ (new Random()).Next(1, 9999);
        }
    }
}