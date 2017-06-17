using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoFacMvc.Models
{
    public interface IService
    {
        string Test();
        
    }

    public interface IMessage
    {
        string Say(string word);
    }
}