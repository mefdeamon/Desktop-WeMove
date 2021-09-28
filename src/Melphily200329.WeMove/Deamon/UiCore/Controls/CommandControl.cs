using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Deamon.UiCore.Controls
{

    public class CommandControl : ContentControl
    {
        static CommandControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandControl), new FrameworkPropertyMetadata(typeof(CommandControl)));
        }
        public CommandControl()
        {
            MouseLeftButtonDown += CommandControlCC_MouseLeftButtonDown;
        }

        private void CommandControlCC_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Command != null)
            {
                if (Command.CanExecute(CommandParameter))
                {
                    Command.Execute(CommandParameter);
                }
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (Command != null)
            {
                if (Command.CanExecute(CommandParameter))
                {
                    Command.Execute(CommandParameter);
                }
            }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand),
                typeof(CommandControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object),
                typeof(CommandControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
    }


}
