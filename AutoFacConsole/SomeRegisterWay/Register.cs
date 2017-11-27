using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Configuration.Core;
using System.Reflection;

namespace AutoFacConsole.SomeRegisterWay
{
    public class Register
    {
        public static ContainerBuilder builder = new ContainerBuilder();
        static void Main2(string[] args)
        {
            register();
            Console.ReadKey();
        }

        /// <summary>
        /// 注册类型时指定Resolve时要调用的构造函数
        /// </summary>
        public static void RegisterByUsingConstructor()
        {
            ////Autofac默认从容器中选择参数最多的构造函数。如果想要选择一个不同的构造函数，就需要在注册的时候就指定它。UsingConstructor方法接受构造函数参数类型数组。
            builder.RegisterType<Computer>().As<Computer>();
            builder.RegisterType<Phone>().As<Phone>();
            builder.RegisterType<User>().UsingConstructor(typeof(Phone));
            using (var container = builder.Build())
            {
                var user = container.Resolve<User>();
                Console.WriteLine(user.ToString());
            }
            
        }
        /// <summary>
        /// 泛型注册
        /// </summary>
        public static void RegisterGeneric()
        {
           
            builder.RegisterGeneric(typeof(List<>)).As(typeof(IList<>)).InstancePerLifetimeScope();
            using (var container = builder.Build())
            {
                var list=container.Resolve<IList<Int32>>();
                list.Add(5);
                Console.WriteLine(list[0]);
            }
        }
        /// <summary>
        /// 注册实例作为服务，一定是单例模式。
        /// </summary>
        public static void RegisterInstance()
        {
            
            ///使用RegisterInstance注册的服务生命周期为SingleInstance，如果指定其他生命周期则报异常。
            builder.RegisterInstance(new User()).As<User>().SingleInstance();
            using (var container = builder.Build())
            {
                var user1 = container.Resolve<User>();
                var user2 = container.Resolve<User>();
                Console.WriteLine(object.ReferenceEquals(user1,user2));
            }
        }
        //lambda注入
        public static void register()
        {
            builder.RegisterType<Computer>();
            builder.Register(c => {
                var u = new User(c.Resolve<Computer>());
                return u;
            });
            //builder.Register(c =>
            //{
            //    var result = new User(5,"wuyong");
            //    Console.WriteLine(result.ToString());
            //    return result;
            //});
            using (var container = builder.Build())
            {
                var user1 = container.Resolve<User>();
                var user2 = container.Resolve<User>();
                Console.WriteLine(user1.ToString());
                Console.WriteLine(object.ReferenceEquals(user1, user2));
            }
        }
        /// <summary>
        /// OnActivated事件中进行一些操作
        /// </summary>
        public static void OnActivated()
        {
            builder.RegisterType<Computer>();
            builder.RegisterType<User>().OnActivated(c => {
                c.Instance.computer = c.Context.Resolve<Computer>();
                Console.WriteLine(c.Instance.ToString());
            });
            using (var container = builder.Build())
            {
                var user = container.Resolve<User>();
               
            }
        }
        //MethodInjection 方法注入，不能自动注入
        public static void MethodInjection()
        {
            builder.Register(c => {
                var u = new User();
                u.say(new Computer());
                return u;
            });
            using (var container = builder.Build())
            {
                var user = container.Resolve<User>();
                Console.WriteLine(user.ToString());
            }
        }
        /// <summary>
        /// 类型注册
        /// </summary>
        public static void RegisterType()
        {
            builder.RegisterType<User>();
            using (var container = builder.Build())
            {
                var user = container.Resolve<User>();
                Console.WriteLine(user.ToString());
            }
        }
        /// <summary>
        /// 类型构造函数参数手动注入
        /// </summary>
        public static void RegisterWithParameters()
        {
            List<NamedParameter> ListNamedParameter = new List<NamedParameter>() { new NamedParameter("_age", 2), new NamedParameter("_name", "wuyong") };
            builder.RegisterType<User>().WithParameters(ListNamedParameter);
            using (var container = builder.Build())
            {
                var user = container.Resolve<User>();
                Console.WriteLine(user.ToString());
            }
        }
        /// <summary>
        /// 类型属性手动注入
        /// </summary>
        public static void RegisterWithProperties()
        {
            List<NamedPropertyParameter> ListNamedProperty = new List<NamedPropertyParameter>() { new NamedPropertyParameter("age", 2), new NamedPropertyParameter("name", "wuyong") };
            builder.RegisterType<User>().WithProperties(ListNamedProperty);
            using (var container = builder.Build())
            {
                var user = container.Resolve<User>();
                Console.WriteLine(user.ToString());
            }
        }
        //PropertiesAutowired,自动属性注入
        //使用PropertiesAutowired也只是能指定某个类会自动进行属性注入，没有一键设置所有类型都会自动注入属性的设置。而且还需要注意一点，设置了自动属性注入后，也不代表所有属性都会自动注入，只有注册到Autofac中的类型才能自动注入
        public static void RegisterWithPropertiesAutowired()
        {
            builder.RegisterType<Computer>();
            builder.RegisterType<User>().PropertiesAutowired();
            using (var container = builder.Build())
            {
                var user = container.Resolve<User>();
                Console.WriteLine(user.ToString());
            }
        }


        //主张，这样之后更新起来只需要替代指定dll即可
        //模块化编程，把职责隔离到不同的dll，在不同的模块中注册不同的类型到autofac中，互不影响。
        /// <summary>
        /// 实现方式：创建模块类继承Module，重写Module类的Load方法，在Load方法中进行自己的类型注册，最后再进行Module的统一注册
        /// </summary>
        public static void RegisterModule()
        {
            builder.RegisterModule<ModuleA>();
            builder.RegisterModule<ModuleB>();
            using (var container = builder.Build())
            {
                var user = container.Resolve<User>();
                Console.WriteLine(user.ToString());
                var Phone = container.Resolve<Phone>();
                Console.WriteLine(Phone.ToString());
            }
        }



        //注册assembly程序集中所有实现了IModule接口的类型（多层继承也算），这样，我们只需要将取出所有程序集，然后通过RegisterAssemblyModules进行一次性注册，就可以自动注册所有Module了。
        /// <summary>
        /// Module程序集注册
        /// </summary>
        public static void RegisterAssemblyModules()
        {
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyModules(assembly);
            //RegisterAssemblyModule还可以指定一个泛型类型：builder.RegisterAssemblyModules<ModuleA>(assembly);
            //这样注册，是指定只注册assembly程序集中继承自ModuleA的Module。
            using (var container = builder.Build())
            {
                var user = container.Resolve<User>();
                Console.WriteLine(user.ToString());
                var Phone = container.Resolve<Phone>();
                Console.WriteLine(Phone.ToString());
            }
        }

        /// <summary>
        /// 在组件component生命周期的不同阶段使用事件。
        ///Autofac暴露五个事件接口供实例的按如下顺序调用
        /// </summary>
        public static void events()
        {
            builder.RegisterType<User>()
            .OnRegistered(e => Console.WriteLine("在注册的时候调用!"))
            .OnPreparing(e => Console.WriteLine("在准备创建的时候调用!"))
            .OnActivating(e => Console.WriteLine("在创建之前调用!"))
            .OnActivated(e => Console.WriteLine("创建之后调用!"))
            .OnRelease(e => Console.WriteLine("在释放占用的资源之前调用!"));
            using (var container = builder.Build())
            {
                var user = container.Resolve<User>();
                Console.WriteLine(user.ToString());
            }
        }
    }
}
