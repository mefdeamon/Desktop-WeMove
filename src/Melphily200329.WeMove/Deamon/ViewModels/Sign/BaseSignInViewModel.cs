#region Deamon
// the app's classes of wemove project
#endregion

using Melphi.Base;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Deamon.ViewModels.Sign
{
    /// <summary>
    /// The base class of sign in information verification 
    /// </summary>
    public abstract class BaseSignInViewModel : NotifyPropertyChanged
    {
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

        /// <summary>
        /// Remember this input and fill in automatically next time
        /// </summary>
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
        /// Show the status of Sign Button is Running or not
        /// </summary>
        public bool IsSigning
        {
            get { return isSigning; }
            set { isSigning = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Validate the command
        /// </summary>
        public abstract ICommand SignCommand { get; set; }

        /// <summary>
        /// Skip to other (like SignUp or SignInEmail) page command
        /// </summary>
        public abstract ICommand GotoCommand { get; set; }

        /// <summary>
        /// Wait for a while, then erase the error message display
        /// </summary>
        /// <param name="ms">The number of milliseconds to wait</param>
        protected void WipeErrorAffterMS(int ms = 2000)
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
