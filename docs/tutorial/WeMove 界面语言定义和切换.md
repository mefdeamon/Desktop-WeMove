## 2021 10 01



### WeMove 界面语言定义和切换



1. 添加string.zh-CN.xaml字体资源文件
2. 将资源文件在APP.xaml文件种引入
3. 界面呈现文本的地方，修改文本的呈现方式为绑定动态资源
4. 在APP.xaml.cs文件种定义公共静态属性Language，当属性发生变化时，修改当前的使用的资源文件



字符串XAML命令空间引用

```
     xmlns:sys="clr-namespace:System;assembly=mscorlib"
```





资源切换

```c#
 					List<ResourceDictionary> dictionaryList = new List<ResourceDictionary>();
                    foreach (ResourceDictionary dictionary in Application.Current.Resources.MergedDictionaries)
                    {
                        dictionaryList.Add(dictionary);
                    }
                    string requestedLanguage = string.Format(@"UiCore/Styles/Languages/String.{0}.xaml", Language);
                    ResourceDictionary resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString.Equals(requestedLanguage));
                    if (resourceDictionary == null)
                    {
                        requestedLanguage = @"UiCore/Styles/Languages/String.zh-CN.xaml";
                        resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString.Equals(requestedLanguage));
                    }
                    if (resourceDictionary != null)
                    {
                        Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
                        Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
                    }
```







