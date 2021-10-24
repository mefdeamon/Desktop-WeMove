using Deamon.ViewModels;
using Deamon.ViewModels.Sign;
using Melphi.Base;
using Melphi.IconSet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deamon
{
    public class ServiceLocator
    {
        public IIconSet IconSource => ServiceProvider.Get<IIconSet>();
        public MaterialSet MaterialSet => ServiceProvider.Get<MaterialSet>();


        public MainViewModel MainViewModel => ServiceProvider.Get<MainViewModel>();
        public SignViewModel SignViewModel => ServiceProvider.Get<SignViewModel>();
        public SignInMailViewModel SignInMailViewModel => ServiceProvider.Get<SignInMailViewModel>();
        public SignInPassViewModel SignInPassViewModel => ServiceProvider.Get<SignInPassViewModel>();
        public SignUpViewModel SignUpViewModel => ServiceProvider.Get<SignUpViewModel>();
        
    }
}
