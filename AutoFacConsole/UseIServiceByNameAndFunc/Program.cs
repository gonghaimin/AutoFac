using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
namespace AutoFacConsole.UseIServiceByName
{
    class Program
    {
        static void Main1(string[] args)
        {
            var builder = new ContainerBuilder();
            //AsImplementedInterfaces表示注册的类型以接口的形式存在，当调用Resolve时要指定注册类型所继承的接口
            builder.RegisterType<OneFoo>().AsImplementedInterfaces();
            builder.RegisterType<OneFoo>().Named<IFoo>("one").As<IFoo>().ExternallyOwned();//ExternallyOwned配置组件，使实例不会被容器处理
            builder.RegisterType<TwoFoo>().Named<IFoo>("two").As<IFoo>();
            builder.Register<Func<string, IFoo>>(c =>
            {
                var cc = c.Resolve<IComponentContext>();
                return named => cc.ResolveNamed<IFoo>(named);
            });
            builder.RegisterType<SomeClass>();
            using (var container = builder.Build())
            {
                var sc = container.Resolve<SomeClass>();
               ///判断服务是否注册。
                if (container.IsRegisteredWithName<IFoo>("two"))
                {
                    sc.foo("two").Say("你好！我是由构造函数自动注入生成的实例对象。");
                }
            }
            Console.ReadKey();
        }
    }
}
