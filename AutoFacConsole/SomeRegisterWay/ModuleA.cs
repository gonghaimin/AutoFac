﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacConsole.SomeRegisterWay
{
    public class ModuleA : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Phone>();
        }
    }
}
