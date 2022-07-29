using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BriefResume.ValidationAttributes
{
    public class ValidationCondition
    {
        //不包含特殊字符
        public static bool HasChinieseWord(string value)
        {
            Regex regex = new Regex(@"^[\u4e00-\u9fa5]{1,}$");//不包含特殊字符
            Match match = regex.Match(value);
            return match.Success;
        }
        //不含空格
        public static bool IsContainSpace(string value)
        {
            if (value != null && value.ToString().Contains(" "))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //是Email格式
        public static bool IsEmail(string value)
        {
            Regex regex = new Regex(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$");//不包含特殊字符
            Match match = regex.Match(value);
            return match.Success;
        }

        //是电话号码格式
        public static bool IsPhoneNumber(string value)
        {
            Regex regex = new Regex(@"^(13[0-9]|14[5|7]|15[0|1|2|3|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9])\d{8}$");//电话号码
            Match match = regex.Match(value);
            return match.Success;
        }



    }
}
