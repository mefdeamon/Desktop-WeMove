#region Deamon
// the app's classes of wemove project
#endregion

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Deamon.UiCore.AttachedProperties
{
    /// <summary>
    /// Focuses (keyboard focus) this element on load
    /// </summary>
    public class IsFocusedProperty : BaseAttachedProperty<IsFocusedProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // If we don't have a control, return
            if (!(sender is Control control))
                return;

            // If TextBox has content default, set the cursor
            if (sender is TextBox textBox)
            {
                // Set the cursor at the end of the last character
                textBox.TextChanged += (s, e) => textBox.SelectionStart = textBox.Text.Length; ;
            }

            // If PasswordBox has content default, set the cursor
            if (sender is PasswordBox passwordBox)
            {
                // Set the cursor at the end of the last character 
                passwordBox.PasswordChanged += (s, e) => passwordBox.GetType()
                           .GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic)
                           .Invoke(passwordBox, new object[] { passwordBox.Password.Length, passwordBox.Password.Length });
            }

            // Focus this control once loaded
            control.Loaded += (s, se) => control.Focus();
        }
    }
}
