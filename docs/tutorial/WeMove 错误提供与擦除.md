# WeMove 错误提供与自动擦除

定义错误提示的UI

```xaml
<WrapPanel Visibility="{Binding HasError, Converter={StaticResource BoolToVisibility}}" Margin="0 50 0 50" HorizontalAlignment="Center" VerticalAlignment="Top" Orientation="Horizontal">
                        <Viewbox Width="25">
                            <Grid Width="20" Height="20">
                                <Ellipse Width="20" Height="20" Fill="Transparent" StrokeThickness="2" Stroke="Red"/>
                                <TextBlock FontSize="15" HorizontalAlignment="Center" VerticalAlignment="Center" Text="!" Foreground="Red"/>
                            </Grid>
                        </Viewbox>

                        <TextBlock Foreground="Red" Margin="10 0 0 0" VerticalAlignment="Center"
                                   Text="{Binding ErrorMessage}" />
                      
                    </WrapPanel>
```

定义绑定的属性

```c#

        private bool hasError;

        /// <summary>
        /// Has error then show the ErrorMessage
        /// </summary>
        public bool HasError
        {
            get { return hasError; }
            set { hasError = value; OnPropertyChanged(); }
        }


        private string errorMessage;

        /// <summary>
        /// the error message
        /// </summary>
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; OnPropertyChanged(); }
        }

```

邮箱格式校验（正则表达式）

```c#
 	public static class CommonHelper
    {
        /// <summary>
        /// 是邮箱
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsEmail(this string source)
        {
            return Regex.IsMatch(source, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", RegexOptions.IgnoreCase);
        }
    }
```

消息自动擦除

```c#
		/// <summary>
        /// 延迟一段时间，擦除错误消息
        /// </summary>
        /// <param name="ms">延迟时间   单位：毫秒</param>
        private void WipeErrorAffterMS(int ms)
        {
            Task.Run(() =>
            {
                Thread.Sleep(ms);
                HasError = false;
                ErrorMessage = "";
            });
        }
```

