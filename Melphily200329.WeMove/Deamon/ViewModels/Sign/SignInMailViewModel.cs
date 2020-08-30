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
    public class SignInMailViewModel : BaseSignInViewModel
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public SignInMailViewModel()
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

        public override ICommand SignCommand { get; set ; }
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
                string sqlCmdStr = @"select Email from user_info";

                var data = mduser.ExecuteSelect(sqlCmdStr);
                if (data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                {
                    var datt = data.Tables[0];

                    if (datt.Rows[0]["Email"].ToString() == email)
                    {
                        App.Current.Dispatcher.Invoke(() => ServiceProvider.Get<SignViewModel>().CurrentView = new Views.Sign.SignInPassView());
                    }
                    else
                    {
                        ErrorMessage = "错误！不存在当前邮箱，请重试！";
                        HasError = true;
                        WipeErrorAffterMS();
                    }
                }

                // 关闭数据库
                mduser.Close();

            });
        }
    }
}
