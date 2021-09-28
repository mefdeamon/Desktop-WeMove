using Deamon.Views.Sign;
using Melphi.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Deamon.ViewModels.Sign
{
    public class SignViewModel : NotifyPropertyChanged
    {

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SignViewModel()
        {
            MinimCommand = new RelayCommand(() =>
            {
                App.Current.MainWindow.WindowState = WindowState.Minimized;
            });

            CloseCommand = new RelayCommand(() =>
            {
                App.Current.MainWindow.Close();
            });

            CurrentView = new SignInMailView();
        }

        private FrameworkElement currentView;

        public FrameworkElement CurrentView
        {
            get { return currentView; }
            set { currentView = value; OnPropertyChanged(); }
        }


        public ICommand MinimCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }


    }
}
