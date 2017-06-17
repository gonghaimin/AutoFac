using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacConsole.SomeRegisterWay
{
    class ModuleB : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            List<NamedPropertyParameter> ListNamedProperty = new List<NamedPropertyParameter>() { new NamedPropertyParameter("age", 2), new NamedPropertyParameter("name", "wuyong") };
            builder.RegisterType<User>().WithProperties(ListNamedProperty);
        }
    }
}
