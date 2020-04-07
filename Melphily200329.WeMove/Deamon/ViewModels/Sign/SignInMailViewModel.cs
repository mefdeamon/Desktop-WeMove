using Melphi.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Deamon.ViewModels.Sign
{
    public class SignInMailViewModel : NotifyPropertyChanged
    {

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SignInMailViewModel()
        {
            SignCommand = new RelayCommand(async () =>
            {
                await RunCommandAsync(() => IsSigning, async () =>
                {

                    // TODO :SQL Query
                    //await Task.Delay(2000);

                    await Task.Run(() =>
                    {
                        // TODO :SQL Query
                        System.Threading.Thread.Sleep(5000);
                        App.Current.Dispatcher.Invoke(() => ServiceProvider.Get<SignViewModel>().CurrentView = new Views.Sign.SignInPassView());
                    });

                });
            });
            GotoCommand = new RelayCommand(() =>
              {
                  ServiceProvider.Get<SignViewModel>().CurrentView = new Views.Sign.SignUpView();
              });
        }


        private string email;

        public string Email
        {
            get { return email; }
            set
            {
                email = value; OnPropertyChanged();
                CanSign = !string.IsNullOrWhiteSpace(email);
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
        public ICommand GotoCommand { get; private set; }



    }
}
