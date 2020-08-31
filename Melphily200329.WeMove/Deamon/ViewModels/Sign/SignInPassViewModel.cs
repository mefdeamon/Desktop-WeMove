#region Deamon
// the app's classes of wemove project
#endregion

using Melphi.Base;
using Melphi.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Deamon.ViewModels.Sign
{
    public class SignInPassViewModel : BaseSignInViewModel
    {
        /// <summary>
        /// Default constructor
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
            GotoCommand = new RelayCommand(() =>
             {
                 ServiceProvider.Get<SignViewModel>().CurrentView = new Views.Sign.SignInMailView();
             });

            Password = "deamon";
        }

        private string password;

        /// <summary>
        /// Password 
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

        public override ICommand SignCommand { get; set; }
        public override ICommand GotoCommand { get; set; }

        /// <summary>
        /// Password verification
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

    }
}
