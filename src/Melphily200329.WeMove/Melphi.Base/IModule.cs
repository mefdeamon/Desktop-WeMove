namespace Melphi.Base
{
    /// <summary>
    /// The module will register into kernel
    /// </summary>
    public interface IModule
    {
        void OnLoad(IBinder binder);
    }
}