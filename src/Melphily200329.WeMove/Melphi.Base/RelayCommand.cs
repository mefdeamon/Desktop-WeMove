#region Melphi.Base
// The base class of WPF desktop software products power by melphily
#endregion

using System;
using System.Windows.Input;

namespace Melphi.Base
{
    // Remark：
    //     If you are using this class in WPF4.5 or above, you need to use the GalaSoft.MvvmLight.CommandWpf
    //     namespace (instead of GalaSoft.MvvmLight.Command). This will enable (or restore)
    //     the CommandManager class which handles automatic enabling/disabling of controls
    //     based on the CanExecute delegate.

    /// <summary>
    ///  A generic command whose sole purpose is to relay its functionality to other objects by invoking delegates. 
    ///  The default return value for the CanExecute method is 'true'. 
    ///  This class allows you to accept command parameters in the Execute and CanExecute callback methods.
    /// </summary>
    /// <typeparam name="T">The type of the command parameter.</typeparam>
    public class RelayCommand<T> : ICommand
    {
        /// <summary>
        /// The execution logic. 
        /// </summary>
        private Action<T> mExecute;

        /// <summary>
        /// The execution status logic. 
        /// </summary>
        private Func<T, bool> mCanExecute;

        /// <summary>
        /// Initializes a new instance of the RelayCommand class that can always execute.
        /// 
        /// Exception:
        ///     T:System.ArgumentNullException:
        ///         If the execute argument is null.
        /// </summary>
        /// <param name="execute">The execution logic. IMPORTANT: If the action causes a closure, you must set keepTargetAlive to true to avoid side effects.</param>
        /// <param name="keepTargetAlive">If true, the target of the Action will be kept as a hard reference, which might cause a memory leak. You should only set this parameter to true if the action is causing a closure. See http://galasoft.ch/s/mvvmweakaction.</param>
        public RelayCommand(Action<T> execute, bool keepTargetAlive = false)
        {
            // TODO: Check the T Type

            this.mExecute = execute;
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// 
        /// Exception:
        ///     T:System.ArgumentNullException:
        ///         If the execute argument is null.
        /// </summary>
        /// <param name="execute">The execution logic. IMPORTANT: If the action causes a closure, you must set keepTargetAlive to true to avoid side effects.</param>
        /// <param name="canExecute">The execution status logic. IMPORTANT: If the func causes a closure, you must set keepTargetAlive to true to avoid side effects.</param>
        /// <param name="keepTargetAlive">If true, the target of the Action will be kept as a hard reference, which might cause a memory leak. You should only set this parameter to true if the action is causing a closure.</param>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute, bool keepTargetAlive = false)
        {
            // TODO: Check the T Type 

            this.mExecute = execute;
            this.mCanExecute = canExecute;
        }

        /// <summary>
        ///  Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed,this object can be set to a null reference</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            if (mCanExecute != null)
                return mCanExecute.Invoke((T)parameter);
            else
                return true;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed,this object can be set to a null reference</param>
        public virtual void Execute(object parameter)
        {
            mExecute.Invoke((T)parameter);
        }

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, null);
        }
    }


    // Remark：
    //     If you are using this class in WPF4.5 or above, you need to use the GalaSoft.MvvmLight.CommandWpf
    //     namespace (instead of GalaSoft.MvvmLight.Command). This will enable (or restore)
    //     the CommandManager class which handles automatic enabling/disabling of controls
    //     based on the CanExecute delegate.

    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other objects by invoking delegates. 
    /// The default return value for the CanExecute method is 'true'.
    /// This class does not allow you to accept command parameters in the Execute and CanExecute callback methods.
    /// </summary>
    public class RelayCommand : ICommand
    {

        /// <summary>
        /// The execution logic. 
        /// </summary>
        private Action mExecute;

        /// <summary>
        /// The execution status logic. 
        /// </summary>
        private Func<bool> mCanExecute;

        /// <summary>
        /// Initializes a new instance of the RelayCommand class that can always execute.
        /// 
        /// Exception:
        ///     T:System.ArgumentNullException:
        ///         If the execute argument is null.
        /// </summary>
        /// <param name="execute">The execution logic. IMPORTANT: If the action causes a closure, you must set keepTargetAlive to true to avoid side effects.</param>
        /// <param name="keepTargetAlive">If true, the target of the Action will be kept as a hard reference, which might cause a memory leak. You should only set this parameter to true if the action is causing a closure.</param>
        public RelayCommand(Action execute, bool keepTargetAlive = false)
        {
            this.mExecute = execute;
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// 
        /// Exception:
        ///     T:System.ArgumentNullException:
        ///         If the execute argument is null.
        /// </summary>
        /// <param name="execute">The execution logic. IMPORTANT: If the action causes a closure, you must set keepTargetAlive to true to avoid side effects.</param>
        /// <param name="canExecute">The execution status logic. IMPORTANT: If the func causes a closure, you must set keepTargetAlive to true to avoid side effects.</param>
        /// <param name="keepTargetAlive">If true, the target of the Action will be kept as a hard reference, which might cause a memory leak. You should only set this parameter to true if the action is causing a closures.</param>
        public RelayCommand(Action execute, Func<bool> canExecute, bool keepTargetAlive = false)
        {
            this.mExecute = execute;
            this.mCanExecute = canExecute;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">This parameter will always be ignored.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            if (mCanExecute != null)
                return this.mCanExecute.Invoke();
            else
                return true;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">This parameter will always be ignored.</param>
        public virtual void Execute(object parameter)
        {
            this.mExecute.Invoke();
        }

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged.Invoke(this, null);
        }
    }

}
