using Melphi.Base;
using Melphi.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deamon.UiCore;

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
        public ICommand GotoCommand { get; private set; }




        /// <summary>
        /// 邮箱登录验证
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
