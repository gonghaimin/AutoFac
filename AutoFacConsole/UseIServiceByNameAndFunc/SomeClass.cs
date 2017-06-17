using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacConsole.UseIServiceByName
{
    public class SomeClass{
        public readonly Func<string, IFoo> foo;
        public SomeClass(Func<string, IFoo> _foo)
        {
            this.foo = _foo;
        }

    }

}
