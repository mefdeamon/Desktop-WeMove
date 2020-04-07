using Ninject.Modules;
using System.Collections.Generic;

namespace Melphi.Base
{

    public class Binder : NinjectModule, IBinder
    {
        private readonly IModule module;

        public Binder(IModule module)
        {
            this.module = module;
        }

        /// <summary>
        /// Binding of singleton type.
        /// </summary>
        /// <typeparam name="T">The singleton type</typeparam>
        public void BindSingleton<T>()
        {
            Bind<T>().ToSelf().InSingletonScope();
        }

        /// <summary>
        /// Binding a specified implementation type to the service interface it inherits.
        /// </summary>
        /// <typeparam name="TFrom">the specified service type (generally it's an Interface)</typeparam>
        /// <typeparam name="TTarget">the specified implementation type (generally it  inherits from the specified service)</typeparam>
        /// <param name="singleton"></param>
        public void Bind<TFrom, TTarget>(bool singleton = true) where TTarget : TFrom
        {
            var binding = Bind<TFrom>().To<TTarget>();

            if (singleton)
                binding.InSingletonScope();
        }

        /// <summary>
        /// Gets an instance of the specified service
        /// </summary>
        /// <typeparam name="T">The service to resolve</typeparam>
        /// <returns>An instance of the service</returns>
        public T Get<T>() => ServiceProvider.Get<T>();

        public override void Load()
        {
            module.OnLoad(this);
        }
    }

}