using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfCorp
{
    /// <summary>
    /// Расширение для TextBox, позволяющее вводить
    /// только цифровую строку заданной длины
    /// </summary>
    public static class TextBoxExtension
    {
        public static void OnlyDigits(this TextBox input, int length = 3)
        {
            bool notADigit = Regex.IsMatch(input.Text, "[^0-9]");

            if (notADigit || input.Text.Length > length)
            {
                input.Text = input.Text.Remove(input.Text.Length - 1);
                input.SelectionStart = input.Text.Length;
            }
        }
    }
}
