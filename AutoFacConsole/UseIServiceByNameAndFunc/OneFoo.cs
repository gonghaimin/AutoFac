using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacConsole.UseIServiceByName
{
    public class OneFoo : IFoo
    {
        public void Say(string word)
        {
            Console.WriteLine("my is one service:"+word);
        }
        public void test()
        {
            Console.WriteLine("Id:"+new Random().Next(1,999));
        }
    }
}
