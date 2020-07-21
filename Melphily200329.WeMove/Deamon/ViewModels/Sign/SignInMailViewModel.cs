using Melphi.Base;
using Melphi.Core;
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

                    await Task.Run(() =>
                    {
                        string ConnectionString = @"server = 127.0.0.1; userid = root; password = deamon; database = wemove; persistsecurityinfo = True;";
                        MySqlDatabaseEntity mduser = new MySqlDatabaseEntity(ConnectionString);

                        // 打开数据库
                        mduser.Open();

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
                                System.Windows.MessageBox.Show("邮箱验证失败，不存在当前输入的邮箱！请重试！");
                            }
                        }

                        // 关闭数据库
                        mduser.Close();

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
