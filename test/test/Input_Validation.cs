using MaterialDesignColors.Recommended;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace test
{
    public class Input_Validation
    {
        public void Input_Validation_TextBox(TextBox textBox)
        {
            string Input = textBox.Text;
            char[] InputChars = { '.',',','/','&','!','@','#','$','%','^','*','(',')','_','-','+','='};
            foreach (var item in InputChars)
            {
                if (Input.Contains(item))
                {
                    MessageBox.Show("Есть запрещенные символы");
                    textBox.Foreground = Brushes.Red;
                    return;
                }
                textBox.Foreground = Brushes.Green;
            }
        }
    }
}
