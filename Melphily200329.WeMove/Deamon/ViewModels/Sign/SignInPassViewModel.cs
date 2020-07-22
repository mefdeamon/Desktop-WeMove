using Melphi.Base;
using Melphi.Core;
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
                    //App.Current.Dispatcher.Invoke(() => ServiceProvider.Get<SignViewModel>().CurrentView = new Views.Sign.SignLoadingView());
                    await SignIn();
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


        private bool hasError = false;

        /// <summary>
        /// has a error occured
        /// </summary>
        public bool HasError
        {
            get { return hasError; }
            set { hasError = value; OnPropertyChanged(); }
        }


        private string errorMessage = "";

        /// <summary>
        /// the error message
        /// </summary>
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; OnPropertyChanged(); }
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


        /// <summary>
        /// 登录密码验证
        /// </summary>
        /// <returns></returns>
        private async Task SignIn()
        {

            if (password.Length < 6)
            {
                ErrorMessage = "错误！邮箱格式不正确，请重试！";
                HasError = true;
                WipeErrorAffterMS();
                return;
            }

            // TODO :SQL Query
            await Task.Run(() =>
            {
                string ConnectionString = @"server = 127.0.0.1; userid = root; password = deamon; database = wemove; persistsecurityinfo = True;";
                MySqlDatabaseEntity mduser = new MySqlDatabaseEntity(ConnectionString);

                try
                {
                    // 打开数据库
                    mduser.Open();
                }
                catch (Exception)
                {
                    // 模拟未连接网络
                    ErrorMessage = "错误！网络连接失败，请重试！";
                    HasError = true;
                    WipeErrorAffterMS();
                    return;
                }

                // sql 查询语句
                string sqlCmdStr = @"select Password from user_info";

                var data = mduser.ExecuteSelect(sqlCmdStr);
                if (data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                {
                    var datt = data.Tables[0];

                    if (datt.Rows[0]["Password"].ToString() == password)
                    {
                        // start up main window
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            Window window = App.Current.MainWindow;
                            App.Current.MainWindow = new Views.MainWindow();
                            window.Close();
                            App.Current.MainWindow.Show();
                        });
                    }
                    else
                    {
                        ErrorMessage = "错误！密码不正确，请重试！";
                        HasError = true;
                        WipeErrorAffterMS();
                        return;
                    }
                }

                // 关闭数据库
                mduser.Close();

            });
        }


        /// <summary>
        /// 等待多少毫秒，然后擦除错误消息展示
        /// </summary>
        private void WipeErrorAffterMS(int ms = 2000)
        {
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(ms);
                ErrorMessage = "";
                HasError = false;
            });
        }

    }
}
