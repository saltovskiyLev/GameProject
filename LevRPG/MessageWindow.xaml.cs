using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LevRPG
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    /// 
    public partial class MessageWindow : Window
    {
        public int ButtonPressed { get; private set; }
        public MessageWindow()
        {
            InitializeComponent();
        }

        public void SetButtonVisibility(int number, Visibility visibility)
        {
            if(number == 1)
            {
                button1.Visibility = visibility;
            }

            if (number == 2)
            {
                button2.Visibility = visibility;
            }

            if (number == 3)
            {
                button3.Visibility = visibility;
            }
        }

        public void SetButtonText(int number, string text)
        {
            if(number == 1)
            {
                tb1.Text = text;
            }

            if (number == 2)
            {
                tb2.Text = text;
            }

            if (number == 3)
            {
                tb3.Text = text;
            }
        }

        public void SetMessageText(string text)
        {
            tbMessage.Text = text;
        }

        private void button3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ButtonPressed = 3;
            this.Close();
        }

        private void button2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ButtonPressed = 2;
            this.Close();

        }

        private void button1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ButtonPressed = 1;
            this.Close();

        }
    }
}
