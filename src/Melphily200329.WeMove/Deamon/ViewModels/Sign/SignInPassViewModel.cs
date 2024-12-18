#region Deamon
// the app's classes of wemove project
#endregion

using Melphi.Base;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
                 ServiceProvider.Get<SignViewModel>().CurrentViewType = SignViewType.SignInMail;
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
            
            var http = "http://localhost:5000";
            var api = "/api/auth/sign-in-password";
            var requestUrl = http + api;

            using (var client = new HttpClient())
            {
                var postData = new
                {
                    Email = ServiceProvider.Get<SignInMailViewModel>().Email,
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
