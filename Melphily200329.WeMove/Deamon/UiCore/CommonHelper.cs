using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Deamon.UiCore
{
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
}
