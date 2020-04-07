namespace Melphi.Base
{

    public interface IBinder
    {
        /// <summary>
        /// Binding of singleton type.
        /// </summary>
        /// <typeparam name="T">The singleton type</typeparam>
        void BindSingleton<T>();

        /// <summary>
        /// Binding a specified implementation type to the service interface it inherits.
        /// </summary>
        /// <typeparam name="TFrom">the specified service type (generally it's an Interface)</typeparam>
        /// <typeparam name="TTarget">the specified implementation type (generally it  inherits from the specified service)</typeparam>
        /// <param name="singleton"></param>
        void Bind<TFrom, TTarget>(bool Singleton = true) where TTarget : TFrom;

        /// <summary>
        /// Gets an instance of the specified service
        /// </summary>
        /// <typeparam name="T">The service to resolve</typeparam>
        /// <returns>An instance of the service</returns>
        T Get<T>();
    }

}