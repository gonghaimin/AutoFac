using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoFacMvc.Models
{
    public interface ILogger
    {
        void Log(string word);
    }
    public class DebLogger : ILogger
    {
        public void Log(string word)
        {
           
        }
    }
}