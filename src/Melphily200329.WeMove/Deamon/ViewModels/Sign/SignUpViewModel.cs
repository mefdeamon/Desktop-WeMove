#region Deamon
// the app's classes of wemove project
#endregion

using Deamon.UiCore;
using Melphi.Base;
using Melphi.Core;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Deamon.ViewModels.Sign
{
    public class SignUpViewModel : BaseSignInViewModel
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public SignUpViewModel()
        {
            SignCommand = new RelayCommand(async () =>
            {
                await RunCommandAsync(() => IsSigning, async () =>
                {
                    await Sign();
                });
            });
            GotoCommand = new RelayCommand(() =>
              {
                  ServiceProvider.Get<SignViewModel>().CurrentViewType = SignViewType.SignInMail;
              });

            Email = "mel@wem.com";

            Username = "Lymehu";
            Password = "";
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


        private string username;

        public string Username
        {
            get { return username; }
            set { Set(ref username, value); }
        }



        public override ICommand SignCommand { get; set; }
        public override ICommand GotoCommand { get; set; }

        /// <summary>
        /// Email verification
        /// </summary>
        /// <returns></returns>
        private async Task Sign()
        {
            // 输入邮箱格式校验
            if (!Email.IsEmail())
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
                string sqlCmdStr = $"select Email from user_info where Email = '{email}'";

                var data = mduser.ExecuteSelect(sqlCmdStr);
                if (data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                {
                    ErrorMessage = "错误！当前邮箱已注册！";
                    HasError = true;
                    WipeErrorAffterMS();

                    return;
                }


                // sql 插入
                string sqlInsert = $"insert into user_info ( Email, Username, Password, CreateDate) values ( '{email}','{username}', '{password}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";
                var rest = mduser.ExecuteNonQuery(sqlInsert);
                if (rest > 0)
                {
                    // start up main window
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        var window = App.Current.MainWindow;
                        App.Current.MainWindow = new Views.MainWindow();
                        window.Close();
                        App.Current.MainWindow.Show();
                    });
                }

                // 关闭数据库
                mduser.Close();

            });
        }
    }
}
