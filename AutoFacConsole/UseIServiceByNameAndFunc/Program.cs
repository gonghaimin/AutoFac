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
        static void Main(string[] args)
        {
            test_InstancePerMatchingLifetimeScope();
            Console.Read();
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
        static void test_InstancePerDependency()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<OneFoo>();
            builder.RegisterType<OneFoo>().InstancePerDependency();
            using(var container = builder.Build())
            {
                using (var scope = container.BeginLifetimeScope())
                {
                    for (var i = 0; i < 5; i++)
                    {
                        //生成5个不同OneFoo的实例。
                        var w = scope.Resolve<OneFoo>();
                        w.test();
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }
        }
        static void test_SingleInstance()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<OneFoo>().SingleInstance();
            using (var container = builder.Build())
            {
                var root = container.Resolve<OneFoo>();
                //我们可以从任何级别的嵌套生命周期解析服务实例任何次数
                using (var scope1 = container.BeginLifetimeScope())
                {
                    for (var i = 0; i < 5; i++)
                    {
                        var w1 = scope1.Resolve<OneFoo>();
                        using (var scope2 = scope1.BeginLifetimeScope())
                        {
                            var w2 = scope2.Resolve<OneFoo>();
                            Console.WriteLine(Object.ReferenceEquals(w2,w1));
                            //root, w1, and w2 总是相同的实例，无论它们
                            //是从任何级别嵌套生命周期任何次数的解析
                        }
                    }
                }
            }
        }

        static void test_InstancePerLifetimeScope()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<OneFoo>().InstancePerLifetimeScope();
            using (var container = builder.Build())
            {
                using (var scope1 = container.BeginLifetimeScope())
                {
                    for (var i = 0; i < 5; i++)
                    {
                        //每次从该scope内解析的服务实例是一样的
                        var w1 = scope1.Resolve<OneFoo>();
                    }
                }
                using (var scope2 = container.BeginLifetimeScope())
                {
                    for (var i = 0; i < 5; i++)
                    {
                        //每次从该scope内解析的服务实例是一样的
                        //w2和w1是不同的实例对象
                        //新的scope，会生成新的服务实例
                        var w2 = scope2.Resolve<OneFoo>();
                    }
                }
            }
        }

        static void test_InstancePerMatchingLifetimeScope()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<OneFoo>().InstancePerMatchingLifetimeScope("myrequest");
            using (var container = builder.Build())
            {
                //使用标签创建lifetime scope
                using (var scope1 = container.BeginLifetimeScope("myrequest"))
                {
                    for (var i = 0; i < 5; i++)
                    {
                        var w1 = scope1.Resolve<OneFoo>();
                        using (var scope2 = scope1.BeginLifetimeScope())
                        {
                            var w2 = scope2.Resolve<OneFoo>();
                            Console.WriteLine(Object.ReferenceEquals(w2, w1));
                            //w1和w2是相同的实例
                            //因为它们是指定scope的单例
                        }
                    }
                }
                using (var scope3 = container.BeginLifetimeScope("myrequest"))
                {
                    for (var i = 0; i < 5; i++)
                    {
                        var w3 = scope3.Resolve<OneFoo>();
                        using (var scope4 = scope3.BeginLifetimeScope())
                        {
                            var w4 = scope4.Resolve<OneFoo>();
                            Console.WriteLine(Object.ReferenceEquals(w4, w3));
                            //w3和w4是相同的实例对象，但是它们不同上面的w1和w2
                        }
                    }
                }
                using (var noTagScope = container.BeginLifetimeScope())
                {
                    //找不到匹配的作用域，会抛出异常
                    // This throws an exception because this scope doesn't
                    // have the expected tag and neither does any parent scope!
                    var fail = noTagScope.Resolve<OneFoo>();
                }
            }
        }
    }
}
