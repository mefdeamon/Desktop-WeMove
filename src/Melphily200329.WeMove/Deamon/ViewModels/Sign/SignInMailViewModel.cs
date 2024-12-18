#region Deamon
// the app's classes of wemove project
#endregion

using Deamon.UiCore;
using Melphi.Base;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using static Deamon.ViewModels.Sign.SignUpViewModel;

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
                  ServiceProvider.Get<SignViewModel>().CurrentViewType = SignViewType.SignUp;
              });

            Email = "mel@wem.com";
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
            var api = "/api/auth/sign-in-email";
            var requestUrl = http + api;

            using (var client = new HttpClient())
            {
                var postData = new
                {
                    Email = email,
                    Password = ""
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
                    App.Current.Dispatcher.Invoke(() => ServiceProvider.Get<SignViewModel>().CurrentViewType = SignViewType.SignInPass);
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

    public class ApiResponse
    {
        public string message { get; set; }
    }
}
