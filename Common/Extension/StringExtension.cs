using System;
using System.Text;
using System.Text.RegularExpressions;

namespace BankReport.Common
{
    public static class StringExtension
    {
        private const char SPACE_FULL_SHAPE = (char)12288;
        private const char SPACE_HALF_SHAPE = (char)32;
        private const char MIN_CHAR_FULL_SHAPE = (char)65281;
        private const char MAX_CHAR_FULL_SHAPE = (char)65374;
        private const int GAP_HALF_FULL_SHAPE = 65248;

        public static string ToHalfShape(this string src)
        {
            if (string.IsNullOrEmpty(src))
            {
                return src;
            }

            StringBuilder sb = new StringBuilder();

            for (int index = 0; index < src.Length; index++)
            {
                char ch = src[index];

                if (SPACE_FULL_SHAPE == ch)
                {
                    sb.Append(SPACE_HALF_SHAPE);
                    continue;
                }

                if (ch >= MIN_CHAR_FULL_SHAPE && ch <= MAX_CHAR_FULL_SHAPE)
                {
                    sb.Append((char)(ch - GAP_HALF_FULL_SHAPE));
                }
                else
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString();
        }
        
        /// <summary>
        /// PAYMUL付款报文，每35个字符截取一个子串
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index">必须是35的倍数</param>
        /// <returns>控制在35个字符及以内的子串</returns>
        public static string PAYMULSubString(this string src, int index = 0)
        {
            int len = src.Length;
            if (index >= len)
            {
                return string.Empty;
            }

            if (index < len)
            {
                return src.Substring(index, (len - index) >= 35 ? 35 : (len - index));
            }

            return string.Empty;
        }

        /// <summary>
        /// get element value by element specificity regular expression
        /// </summary>
        /// <param name="src">source string</param>
        /// <param name="regexString">regular expression string</param>
        /// <param name="startRemoveLength">the length of head need remove</param>
        /// <param name="removeTheLastChar">remove the last specificity char</param>
        /// <returns>pure string value</returns>
        public static string EDIFACTElementValue(this string src, string regexString, int startRemoveLength, bool removeTheLastChar = true)
        {
            Regex regex = new Regex(regexString);
            string rslt = regex.Match(src).Value;
            if (string.IsNullOrEmpty(rslt))
            {
                return string.Empty;
            }
            if (removeTheLastChar)
            {
                rslt = rslt.Remove(rslt.Length - 1);
            }
            if (rslt.Length >= startRemoveLength)
            {
                rslt = rslt.Remove(0, startRemoveLength);
            }
            return rslt;
        }

        public static string EDIFACTElementValue(this string src, string regexString, int startRemoveLength, int endRemoveLength)
        {
            Regex regex = new Regex(regexString);
            string rslt = regex.Match(src).Value;
            if (string.IsNullOrEmpty(rslt))
            {
                return string.Empty;
            }
            if (rslt.Length >= endRemoveLength)
            {
                rslt = rslt.Remove(rslt.Length - endRemoveLength);
            }
            if (rslt.Length >= startRemoveLength)
            {
                rslt = rslt.Remove(0, startRemoveLength);
            }
            return rslt;
        }

        /// <summary>
        /// split a string with indicate splitor, but each item will be start with the splitor except the first one
        /// </summary>
        /// <param name="src">the string you want to split</param>
        /// <param name="splitor">splitor</param>
        /// <param name="option">StringSplitOptions</param>
        /// <returns></returns>
        public static string[] SplitItemWithSplitor(this string src,string splitor, StringSplitOptions option = StringSplitOptions.None)
        {
            if (string.IsNullOrEmpty(src) || string.IsNullOrEmpty(splitor))
            {
                return null;
            }
            string[] srcArr = src.Split(new string[] { splitor }, option);
            int len = srcArr.Length;
            //ignore first item
            for (int i = 1; i < len; i++)
            {
                srcArr[i] = splitor + srcArr[i];
            }
            return srcArr;
        }

        public static string SqlInjectionPrevent(this string src)
        {
            string regexStr = @"INSERT|UPDATE|CREATE|DELETE|TRUNCATE|SELECT|DROP|ALTER|'";
            return new Regex(regexStr).Replace(src, m => { return string.Empty; });
        }

        public static string SubStr(this string src,int strLen)
        {
            if (src.Length > strLen)
            {
                return src.Substring(0, strLen);
            }
            else
            {
                return src;
            }
        }

        public static string ContentTrim(this string src)
        {
            return Regex.Replace(src,@"[ \t\r\n]",m=>string.Empty);
        }
    }
}
