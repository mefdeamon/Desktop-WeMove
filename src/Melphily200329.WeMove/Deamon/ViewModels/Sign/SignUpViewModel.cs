#region Deamon
// the app's classes of wemove project
#endregion

using Deamon.UiCore;
using Melphi.Base;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System;

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

            var http = "http://localhost:5000";
            var api = "/api/auth/sign-up";
            var requestUrl = http + api;

            using (var client = new HttpClient())
            {
                var postData = new
                {
                    Email = email,
                    UserName = username,
                    Password = password
                };
                var json = JsonSerializer.Serialize(postData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                try
                {
                    response = await client.PostAsync(requestUrl, content);
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                    HasError = true;
                    WipeErrorAffterMS();
                    return;
                }

                if (response.IsSuccessStatusCode)
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
                    var contentString = await response.Content.ReadAsStringAsync();
                    ApiResponse apiResponse = null;
                    try
                    {
                        apiResponse = JsonSerializer.Deserialize<ApiResponse>(contentString);
                        ErrorMessage = apiResponse == null ? "response content json deserialize error" : apiResponse.message;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                    }

                    HasError = true;
                    WipeErrorAffterMS();
                    return;
                }
            }
        }
    }
}
