using Deamon.Models;
using Melphi.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Deamon
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Language = "en-US";
           // Language = "zh-CN";

            // Register modules into DI container
            ServiceProvider.LoadModule(new MainModule());

            // Load main view
            Views.Sign.SignWindow window = new Views.Sign.SignWindow();
            App.Current.MainWindow = window;
            App.Current.MainWindow.Show();

        }


        private static string language;

        public static string Language
        {
            get { return language; }
            set
            {
                if (language != value)
                {
                    language = value;

                    List<ResourceDictionary> dictionaryList = new List<ResourceDictionary>();
                    foreach (ResourceDictionary dictionary in Application.Current.Resources.MergedDictionaries)
                    {
                        dictionaryList.Add(dictionary);
                    }
                    string requestedLanguage = string.Format(@"/UiCore/Styles/Languages/String.{0}.xaml", Language);
                    ResourceDictionary resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString.Equals(requestedLanguage));
                    if (resourceDictionary == null)
                    {
                        requestedLanguage = @"/UiCore/Styles/Languages/String.zh-CN.xaml";
                        resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString.Equals(requestedLanguage));
                    }
                    if (resourceDictionary != null)
                    {
                        Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
                        Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
                    }




                }
            }
        }

    }
}
