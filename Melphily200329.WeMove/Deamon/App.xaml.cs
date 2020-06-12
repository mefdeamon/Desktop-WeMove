using Deamon.Models;
using Melphi.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Deamon
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // 加了个测试注释
            // Register modules into DI container
            ServiceProvider.LoadModule(new MainModule());

            // Load main view
            Views.Sign.SignWindow window = new Views.Sign.SignWindow();
            App.Current.MainWindow = window;
            App.Current.MainWindow.Show();

        }
    }
}
