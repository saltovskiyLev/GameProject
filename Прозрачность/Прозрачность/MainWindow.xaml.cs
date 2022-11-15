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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace Прозрачность
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int counter = 100;
        Timer timer;
        Action timerAction;
        public MainWindow()
        {
            InitializeComponent();
            timerAction += SetOpacity;
            TimerCallback tc = new TimerCallback(CallBack);
            timer = new Timer(tc, null, 0, 10);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tb.Opacity = 1;
        }

        void SetOpacity()
        {
            if(tb.Opacity > 0)
            {
                tb.Opacity -= 0.01;
            }
        }

        void CallBack(object state = null)
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke(timerAction);
            }
        }
    }
}
