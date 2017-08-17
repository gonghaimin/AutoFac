using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacConsole
{
    public class ClassArg1 { }

    public class ClassArg2 { }

    public class ClassTarget
    {
        public delegate ClassTarget Factory(ClassArg2 arg2);


        public ClassTarget(ClassArg1 arg1, ClassArg2 arg2) { }


        public bool IsInitialized { get; set; }
    }
    public class tst
    {
        static void Main(string[] args)
        {
            ResolveFuncTest.Test();
            Console.ReadKey();
        }
    }
    public static class ResolveFuncTest
    {
        public static void Initialize(ClassTarget target)
        {
            target.IsInitialized = true;
        }

        public static void Test()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ClassArg1>().AsSelf().SingleInstance();

            builder.RegisterType<ClassTarget>().OnActivated(args => Initialize(args.Instance));

            var context = builder.Build();
            using (var scope = context.BeginLifetimeScope())
            {
                var factory = scope.Resolve<ClassTarget.Factory>();
                var classTarget = factory(new ClassArg2());
                Console.WriteLine("ClassTarget is initialized? {0}", classTarget.IsInitialized);
            }
        }
    }
}
