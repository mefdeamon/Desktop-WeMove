using Deamon.Models;
using Melphi.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Deamon.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MainViewModel()
        {
            MinimCommand = new RelayCommand(() =>
             {
                 App.Current.MainWindow.WindowState = WindowState.Minimized;
             });

            CloseCommand = new RelayCommand(() =>
            {
                App.Current.MainWindow.Close();
            });

        }

        public ICommand MinimCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }


    }
}
