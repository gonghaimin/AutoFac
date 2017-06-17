using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacConsole.UseIServiceByName
{
    public class TwoFoo : IFoo
    {
        public void Say(string word)
        {
            Console.WriteLine("my is two service:" + word);
        }
    }
}
