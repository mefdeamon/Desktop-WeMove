## 2020 08 30

### WeMove 登录信息验证调整与焦点获取





1.将登录验证类整理，提高代码复用

我们验证密码和邮箱的窗口有部分重复的参数，将其规整到父类中，从而提高代码复用。

1.1添加抽象父类BaseSignInViewModel，将共有的信息集中起来

```c#
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
```

1.2修改对应SignInPass和Mail的ViewModel继承关系。

2调整界面，默认焦点到输入框，与默认ENter在登录按钮上

2.1添加IsFocusedProperty附加属性，用于设置控件的Focus状态

2.2设置控件可以获取焦点，通过tab切换        <Setter Property="Focusable" Value="True"/>

当获取焦点时，通过变色提示用户【button checkbox】

3.将注释统一成英文（直接使用百度翻译）





WPF WeMove 验证结构调整与焦点获取

1.调整类之间结构，提高代码复用
2.TextBox、Password等控件默认获取焦点
3.一般控件获取焦点后样式设置。
项目开源地址：https://github.com/mefdeamon/Desktop-WeMove


