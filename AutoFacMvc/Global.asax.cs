using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoFacMvc.Models;

namespace AutoFacMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // 创建一个容器
            var builder = new ContainerBuilder();
            // 注册所有的Controller
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            builder.RegisterType<MyService>().InstancePerRequest();
            builder.RegisterType<User>().WithProperty("name", "ghm").As<User>();
            // RegisterType方式：
            // builder.RegisterType<InjectionTestService>().AsSelf().InstancePerDependency();

            // Register方式：
            //builder.Register(c => new InjectionTestService()).AsSelf().InstancePerDependency();
            // 自动注入的方式，不需要知道具体类的名称

            // builder.RegisterInstance()以实例注册的对象是单例模式

            // 获取包含继承了IService接口类的程序集
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t => t.GetInterfaces().Contains(typeof(IService))).InstancePerLifetimeScope();
            // 获取包含继承了ILogger接口类的程序集

            // builder.RegisterType<DebLogger>().As<ILogger>().InstancePerDependency();
            typeof(ILogger).Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ILogger))).ToList().ForEach(t => {
                builder.RegisterType(t).As<ILogger>().InstancePerDependency();
            });
         
            ///注册服务，用name区分不同的服务，这里使用自定义属性来设置name值。
            typeof(IMessage).Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IMessage))).ToList().ForEach(messageservice =>{
                builder.RegisterType(messageservice).Named<IMessage>(
                 (
                    // 获取类自定义属性，其枚举属性值作为服务的name。
                    (messageservice.GetCustomAttributes(typeof(MessagePlatformAttribute), false).FirstOrDefault() as MessagePlatformAttribute).Platform.ToString()
                 )).InstancePerRequest(); });
            ///注册委托
            builder.Register<Func<MPlatform, IMessage>>(c =>
            {
                var ic = c.Resolve<IComponentContext>();
                return name => ic.ResolveNamed<IMessage>(name.ToString());
            });
            //注册模型绑定器
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();
            //注册网络抽象
            builder.RegisterModule<AutofacWebTypesModule>();
            // 确保对视图页面的依赖注入可用
            builder.RegisterSource(new ViewRegistrationSource());
            //确保对行为筛选器的属性注入可用,添加属性到你的筛选器同时任何在容器中注册的匹配依赖项都会被注入到这些属性中。
            builder.RegisterFilterProvider();
            builder.RegisterType<DBContext>().AsSelf().PropertiesAutowired().WithParameter("name", "111").InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Repository<>)).AsSelf().PropertiesAutowired().InstancePerLifetimeScope();
            builder.RegisterType<User>();
            // 把容器装入到微软默认的依赖注入容器中
            var container = builder.Build();
            var aaa = container.Resolve<DBContext>();
            var aa=container.Resolve<Repository<User>>();
            var resolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(resolver);

           
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
