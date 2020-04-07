using Melphi.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Deamon.ViewModels.Sign
{
    public class SignInPassViewModel : NotifyPropertyChanged
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SignInPassViewModel()
        {
            SignCommand = new RelayCommand(async () =>
            {
                await RunCommandAsync(() => IsSigning, async () =>
                {

                    App.Current.Dispatcher.Invoke(() => ServiceProvider.Get<SignViewModel>().CurrentView = new Views.Sign.SignLoadingView());


                    // TODO :SQL Query
                    //await Task.Delay(2000);

                    await Task.Run(() =>
                    {
                        // TODO :SQL Query

                        Task.Delay(1000).ContinueWith(t =>
                        {
                            // start up main window
                            App.Current.Dispatcher.Invoke(() =>
                            {
                                Window window = App.Current.MainWindow;

                                App.Current.MainWindow = new Views.MainWindow();

                                window.Close();

                                App.Current.MainWindow.Show();
                            });
                        });

                    });

                });
            });
            BackCommand = new RelayCommand(() =>
             {
                 ServiceProvider.Get<SignViewModel>().CurrentView = new Views.Sign.SignInMailView();
             });
        }


        private string password;

        /// <summary>
        /// password 
        /// </summary>
        public string Password
        {
            get { return password; }
            set
            {
                password = value; OnPropertyChanged();
                CanSign = !string.IsNullOrWhiteSpace(password);
            }
        }


        private bool needRemember;

        public bool NeedRemember
        {
            get { return needRemember; }
            set { needRemember = value; OnPropertyChanged(); }
        }


        private bool canSign;

        /// <summary>
        /// Is ture U Can Sign In
        /// </summary>
        public bool CanSign
        {
            get { return canSign; }
            set { canSign = value; OnPropertyChanged(); }
        }

        private bool isSigning;

        /// <summary>
        /// show the status of Sign Button is Running or not
        /// </summary>
        public bool IsSigning
        {
            get { return isSigning; }
            set { isSigning = value; OnPropertyChanged(); }
        }

        public ICommand SignCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
    }
}
