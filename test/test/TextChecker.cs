using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace test
{
     static class TextChecker
    {
        
        static public int CheckInt(string text)
        {
            bool valid = int.TryParse(text, out int value);

            if (valid)
            {
                return value;
            } else
            {
                throw new Exception();
            }
        }

        static public string CheckCyrillic(string text)
        {
            Regex regex = new Regex("[АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя]*");
            MatchCollection matches = regex.Matches(text);
            if (matches[0].ToString() == text)
            {
                return text;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
