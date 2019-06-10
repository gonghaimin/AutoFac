using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoFacMvc.Models
{
    public class User
    {
        public string name { get; set; }
    }
    public class DBContext
    {
        public DBContext(string name)
        {
            _name = name;
        }
        public string _name;
    }
    public class Repository<T> where T: new()
    {
        public DBContext db { get; set; }
        public Repository()
        {
           
        }
    }
}