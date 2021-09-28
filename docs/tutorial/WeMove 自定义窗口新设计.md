## 2020 08 31



### WeMove 自定义窗口新设计





1.最大化，窗口化

1.1解决最大化，窗口不能正确在任务栏之上展示

添加WindowResizer类【别人的】

实现一个可以控制设置的自定义ViewModel【根据别人改的】

修改Style，给界面添加一个雏形



1.2通过附加依赖属性，在Style中提示当前处于什么状态，并做对应UI展示

IsMaximizedProperty附加属性

在BaseWindowModel中添加IsMaximized属性用于获取当前窗口状态，展示对应信息

功能按钮提示功能，与动画

1.3添加窗体自动缩放和调整大小功能

1.4Box光标焦点问题

TextBox

```
 if (sender is TextBox textBox)
            {
                // Set the cursor at the end of the last character 
                textBox.SelectionStart = textBox.Text.Length;
            }
```

PasswordBox

```
 if (sender is PasswordBox passwordBox)
            {
                // Set the cursor at the end of the last character
                passwordBox.GetType()
                           .GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic)
                           .Invoke(passwordBox, new object[] { passwordBox.Password.Length, passwordBox.Password.Length });
            }
```





WPF WeMove 自定义窗口样式设计2

1.使用WindowResizer类实现窗口的Dock样式
2.最大化、窗口化图标动画提示。
3.TextBox与PasswordBox控件光标焦点。
项目开源地址：https://github.com/mefdeamon/Desktop-WeMove


