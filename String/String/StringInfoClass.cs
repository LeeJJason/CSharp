using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace String
{
    /*
     * 
    Unicode 编码范围 0x00000 - 0x10FFFF，包含 17 个平面，每个平面 65536 个字符。
    UCS-2 规定 0xD800 - 0xDFFF 之间不包含有效字符。高位代理 0xD800 - 0xDBFF，低位代理 0xDC00 - 0xDFFF。
    UTF-8 首字节的二进制中，第一个0出现前有多少个1则有多少个字节数据。


        Char中判断代理的源码
        public static bool IsSurrogate(char c)
        {
            if (c >= '\ud800')
            {
                return c <= '\udfff';
            }
            return false;
        }

        public static bool IsHighSurrogate(char c)
        {
            if (c >= '\ud800')
            {
                return c <= '\udbff';
            }
            return false;
        }


        public static bool IsLowSurrogate(char c)
        {
            if (c >= '\udc00')
            {
                return c <= '\udfff';
            }
            return false;
        }
      
     */

    /// <summary>
    /// 参考 https://zhuanlan.zhihu.com/p/88767935
    /// 
    /// 
    /// </summary>
    class StringInfoClass
    {
        public static void LoadString()
        {
            string str = "AB吉𠮷😁👨‍👩‍👧‍👦";

            Char[] chars = str.ToCharArray();
            System.Diagnostics.Debug.WriteLine("Char Length : " + chars.Length + " => " + StringToHex(str));

            StringInfo stringInfo = new StringInfo(str);

            System.Diagnostics.Debug.WriteLine(stringInfo.String + " LengthInTextElements : " + stringInfo.LengthInTextElements);

            System.Diagnostics.Debug.WriteLine("GetNextTextElement : " + StringInfo.GetNextTextElement(str));

            //部分错误
            System.Diagnostics.Debug.WriteLine("GetNextTextElement By Index: ");
            for (int i = 0; i < stringInfo.LengthInTextElements; ++i)
            {
                string c = StringInfo.GetNextTextElement(str, i);
                System.Diagnostics.Debug.WriteLine("\t" + i + " => " + c + "[" + StringToHex(c) + "]");
            }

            //仅有的正确处理
            System.Diagnostics.Debug.WriteLine("TextElementEnumerator : ");
            TextElementEnumerator textElementEnumerator = StringInfo.GetTextElementEnumerator(str);
            int index = 0;
            while (textElementEnumerator.MoveNext())
            {
                string c = textElementEnumerator.GetTextElement();
                System.Diagnostics.Debug.WriteLine("\t" + index++ + " : " + textElementEnumerator.ElementIndex + " => " + c + "[" + StringToHex(c) + "]");
            }

            //有问题的emoji处理
            System.Diagnostics.Debug.WriteLine("ParseCombiningCharacters : ");
            int[] indexs = StringInfo.ParseCombiningCharacters(str);
            for (int i = 0; i < indexs.Length; ++i)
            {
                string c = stringInfo.SubstringByTextElements(indexs[i]);
                System.Diagnostics.Debug.WriteLine("\t" + i + "<" + indexs[i] + ">" + " => " + c + "[" + StringToHex(c) + "]");
            }
        }

        public static void ParseString()
        {
            string str = "AB吉𠮷😁👨‍👩‍👧‍👦";
            System.Diagnostics.Debug.WriteLine("<" + str + ">" + " Length : " + str.Length);
            int i = 0;
            do
            {

                int stage = char.IsHighSurrogate(str[i]) ? 2 : 1;
                string c = str.Substring(i, stage);
                System.Diagnostics.Debug.WriteLine(i + ":" + stage + " => " + c + "[" + StringToHex(c) + "]");
                i += stage;
            } while (i < str.Length);
        }

        public static void SplitString()
        {
            string str = "AB吉𠮷😁👨‍👩‍👧‍👦";
            System.Diagnostics.Debug.WriteLine("<" + str + ">" + " Length : " + str.Length);
            int i = 0;
            do
            {
                int length = 0;
                if (char.IsHighSurrogate(str[i]))
                {
                    do
                    {
                        length += 2;
                        if (i + length < str.Length && str[i + length] == 0x200D)
                        {
                            length += 1;
                        }
                        else
                        {
                            break;
                        }
                    } while (i + length < str.Length);
                    
                }
                else
                {
                    length = 1;
                }

                string c = str.Substring(i, length);
                System.Diagnostics.Debug.WriteLine(i + ":" + length + " => " + c + "[" + StringToHex(c) + "]");
                i += length;
            } while (i < str.Length);
        }


        private static StringBuilder builder = new StringBuilder();
        private static string StringToHex(string str)
        {
            builder.Length = 0;
            builder.Append("0x");
            byte[] bytes = Encoding.Unicode.GetBytes(str);
            byte[] uf8bytes = Encoding.UTF8.GetBytes(str);
            for (int i = 0; i < bytes.Length; ++i)
            {
                builder.AppendFormat("{0:X2}", bytes[i]);
            }

            builder.Append(" | 0x");
            for (int i = 0; i < uf8bytes.Length; ++i)
            {
                builder.AppendFormat("{0:X2}", uf8bytes[i]);
            }

            return builder.ToString();
        }
    }
}
