using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace Deamon.UiCore.ValueConverters
{
    /// <summary>
    /// 将布尔值转换成其他类型，需要提供两个 Object 类型的真值和假值
    /// </summary>
    public class BooleanToObjectConverter : IValueConverter
    {
        public object FalseValue { get; set; }
        public object TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // 没有参数的情况下是正向的，T-T  F-F
            if (parameter == null)
                return Equals(value, true) ? TrueValue : FalseValue;
            // 有参数的情况下是反向的，  T-F  F-T
            else
                return Equals(value, true) ? FalseValue : TrueValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Equals(value, TrueValue) ? true : Equals(value, FalseValue) ? false : (bool?)null;
        }
    }
}
