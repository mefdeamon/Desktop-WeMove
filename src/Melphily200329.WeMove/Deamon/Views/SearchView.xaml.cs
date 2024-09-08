using Deamon.Models;
using Melphi.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Deamon.Views
{
    /// <summary>
    /// SearchView.xaml 的交互逻辑
    /// </summary>
    public partial class SearchView : UserControl
    {
        public SearchView()
        {
            InitializeComponent();
            DataContext = new SearchViewModel();
        }
    }
    public class IconModel
    {
        public string IconName { get; set; }

        public string IconData { get; set; }
    }

    public class SearchViewModel : NotifyPropertyChanged
    {
        /// <summary>
        /// 本地服务对象
        /// </summary>
        ServiceLocator Locator = new ServiceLocator();

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SearchViewModel()
        {
            SearchCommand = new RelayCommand(() =>
            {
                MessageBox.Show(SearchText);
            });

            ChooseCommand = new RelayCommand<IconModel>(t =>
            {
                SelectedIcon = t;
            });



            // 初始化数据源
            var list = FindDititsInfo(Locator.IconSource);
            OriginalIcons = list;

            // 初始化界面
            SearchText = "";
        }

        /// <summary>
        /// 原始数据
        /// </summary>
        IEnumerable<IconModel> OriginalIcons;


        private string searchText;

        /// <summary>
        /// 当前搜索文本
        /// </summary>
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                RaisePropertyChanged(nameof(SearchText));
                OnSearchTextChanged();
            }
        }

        /// <summary>
        /// 当搜索文本发生变化时，修改搜索匹配的图标集合
        /// </summary>
        private void OnSearchTextChanged()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                SearchedIcons = new ObservableCollection<IconModel>(OriginalIcons);
                return;
            }

            // 根据输入字符，检索原始数据中匹配的图标，并更新界面的绑定数据源
            SearchedIcons = new ObservableCollection<IconModel>(OriginalIcons.Where(t => t.IconName.ToLowerInvariant().Contains(SearchText.ToLowerInvariant())));
        }


        private ObservableCollection<IconModel> searchedIcons;

        /// <summary>
        /// 当前搜索匹配的图标集合
        /// </summary>
        public ObservableCollection<IconModel> SearchedIcons

        {
            get { return searchedIcons; }
            set
            {
                Set(ref searchedIcons, value);
            }
        }

        private IconModel selectedIcon;

        /// <summary>
        /// 当前选中的图标
        /// </summary>
        public IconModel SelectedIcon
        {
            get { return selectedIcon; }
            set { selectedIcon = value; OnPropertyChanged(); }
        }


        /// <summary>
        /// 搜索命令
        /// </summary>
        public RelayCommand SearchCommand { get; private set; }


        /// <summary>
        /// 选中命令
        /// </summary>
        public RelayCommand<IconModel> ChooseCommand { get; private set; }




        /// <summary>
        /// 获取 model 对象中的属性信息
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="model">对象</param>
        /// <returns></returns>
        List<IconModel> FindDititsInfo<T>(T model)
        {
            List<IconModel> icons = new List<IconModel>();
            var newType = model.GetType();
            foreach (var item in newType.GetRuntimeProperties())
            {
                IconModel icon = new IconModel();
                icon.IconName = item.Name;
                icon.IconData = item.GetValue(model).ToString();
                icons.Add(icon);
            }
            return icons;
        }

    }
}
