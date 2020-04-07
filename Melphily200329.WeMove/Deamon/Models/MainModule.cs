using Deamon.ViewModels;
using Deamon.ViewModels.Sign;
using Melphi.Base;
using Melphi.IconSet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deamon.Models
{
    public class MainModule : IModule
    {
        /// <summary>
        /// Register module into DI container
        /// </summary>
        /// <param name="binder"></param>
        public void OnLoad(IBinder binder)
        {

            binder.Bind<IIconSet, FdoiIconSet>();

            binder.BindSingleton<MainViewModel>();

            binder.BindSingleton<SignViewModel>();

            binder.BindSingleton<SignInMailViewModel>();

            binder.BindSingleton<SignInPassViewModel>();
        }
    }
}
