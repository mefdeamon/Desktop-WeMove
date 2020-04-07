#region Melphi.Base
// The base class of WPF desktop software products power by melphily
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Melphi.Base
{
    /// <summary>
    /// A base class for objects of which the properties must be observable.
    /// </summary>
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs after a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Verifies that a property name exists in this ViewModel.
        /// This method can be called  before the property is used, for instance before calling RaisePropertyChanged. 
        /// It avoids errors when a property name is changed but some places are missed.
        /// </summary>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the PropertyChanged event if needed.
        /// </summary>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            RaisePropertyChanged(propertyName);
        }

        /// <summary>
        ///  Assigns a new value to the property. 
        ///  Then, raises the PropertyChanged event if needed.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="Value">The property's value after the change occurred.</param>
        /// <param name="PropertyName">(optional) The name of the property that changed.</param>
        /// <returns></returns>
        protected bool Set<T>(ref T field, T Value, [CallerMemberName] string PropertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, Value))
                return false;

            field = Value;

            RaisePropertyChanged(PropertyName);

            return true;
        }

        /// <summary>
        /// Raises all of propertys PropertyChanged event if needed.
        /// </summary>
        protected void RaiseAllChanged()
        {
            RaisePropertyChanged("");
        }


        protected object mPropertyValueCheckLock = new object();

        protected async Task RunCommandAsync(Expression<Func<bool>> updatingFlag, Func<Task> action)
        {
            // Lock to ensure single access to check
            lock (mPropertyValueCheckLock)
            {
                // Check if the flag property is true (meaning the function is already running)
                if (updatingFlag.GetPropertyValue())
                    return;

                // Set the property flag to true to indicate we are running
                updatingFlag.SetPropertyValue(true);
            }

            try
            {
                // Run the passed in action
                await action();
            }
            finally
            {
                // Set the property flag back to false now it's finished
                updatingFlag.SetPropertyValue(false);
            }
        }
    }
}
