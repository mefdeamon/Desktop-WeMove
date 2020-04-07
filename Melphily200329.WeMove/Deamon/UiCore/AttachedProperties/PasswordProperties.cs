using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Deamon.UiCore.AttachedProperties
{
    public class BoundPasswordProperty : BaseAttachedProperty<BoundPasswordProperty, string>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox box = sender as PasswordBox;

            if (box == null)
            {
                return;
            }

            box.PasswordChanged += Box_PasswordChanged;

            if (!UpdatingProperty.GetValue(sender))
                box.Password = e.NewValue.ToString();

            box.PasswordChanged -= Box_PasswordChanged;
        }

        private void Box_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox box = sender as PasswordBox;

            box.SetValue(UpdatingProperty.ValueProperty, true);
            box.SetValue(ValueProperty, box.Password);
            box.SetValue(UpdatingProperty.ValueProperty, false);
        }
    }

    public class NeedCaptureProperty : BaseAttachedProperty<NeedCaptureProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox box = sender as PasswordBox;

            if (box == null)
            {
                return;
            }

            if ((bool)e.OldValue)
            {
                box.PasswordChanged -= Box_PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                box.PasswordChanged += Box_PasswordChanged;
                HasPasswordProperty.SetValue(box);
            }
        }

        private void Box_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox box = sender as PasswordBox;

            box.SetValue(UpdatingProperty.ValueProperty, true);
            box.SetValue(BoundPasswordProperty.ValueProperty, box.Password);
            box.SetValue(UpdatingProperty.ValueProperty, false);

            HasPasswordProperty.SetValue(box);
        }
    }

    public class HasPasswordProperty : BaseAttachedProperty<HasPasswordProperty, bool>
    {
        public static void SetValue(DependencyObject sender)
        {
            SetValue(sender, (sender as PasswordBox).SecurePassword.Length > 0);
        }
    }

    public class UpdatingProperty : BaseAttachedProperty<UpdatingProperty, bool>
    {

    }
}
