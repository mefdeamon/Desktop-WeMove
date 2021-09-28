using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Melphi.Base
{
    public class ServiceProvider
    {
        static IKernel Kernel { get; } = new StandardKernel();


        public static void LoadModule(IModule module)
        {
            Kernel.Load(new Binder(module));
        }
        public static T Get<T>() => Kernel.Get<T>();
    }


}
