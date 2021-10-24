using Deamon.Views.Sign;
using Melphi.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Deamon.ViewModels.Sign
{

    public enum SignViewType
    {
        None,
        SignInMail,
        SignInPass,
        SignUp
    }

    public class SignViewModel : NotifyPropertyChanged
    {

        private SignViewType currentViewType;

        public SignViewType CurrentViewType
        {
            get { return currentViewType; }
            set
            {
                if (Set(ref currentViewType, value))
                {
                    switch (currentViewType)
                    {
                        case SignViewType.SignInMail:
                            CurrentView = ServiceProvider.Get<SignInMailView>();
                            WindowHeight = 462;
                            break;
                        case SignViewType.SignInPass:
                            CurrentView = ServiceProvider.Get<SignInPassView>(); 
                            WindowHeight = 462;
                            break;
                        case SignViewType.SignUp:
                            CurrentView = ServiceProvider.Get<SignUpView>() ;
                            WindowHeight = 562;
                            break;
                        default:
                            break;
                    }
                }
            }
        }


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

            CurrentViewType = SignViewType.SignInMail;
        }

        private FrameworkElement currentView;

        public FrameworkElement CurrentView
        {
            get { return currentView; }
            private set { currentView = value; OnPropertyChanged(); }
        }


        private double windowHeight = 462;

        public double WindowHeight
        {
            get { return windowHeight; }
            set { Set(ref windowHeight, value); }
        }

        public ICommand MinimCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }


    }
}
