using MessangerCore;
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
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace MessangerClient
{
    /// <summary>
    /// Логика взаимодействия для invitesWindow.xaml
    /// </summary>
    /// 

    public partial class invitesWindow : Window
    {
        string SessionKey;
        ObservableCollection<string> Invites;
        public invitesWindow(string sessionKey)
        {
            //TODO: При открытии окна запрашивать от сервера все инвайты и выводить их на экран
            InitializeComponent();

            string Result;
            SessionKey = sessionKey;

            Network.SendRequest("http://localhost:8000/GetInvites", "POST",
            Encoding.GetEncoding("utf-8").GetBytes(SessionKey), "text/plain", out Result);

            List<string> ResultList = JsonConvert.DeserializeObject<List<string>>(Result);

            if(ResultList == null)
            {
                ResultList = new List<string>();
            }

            Invites = new ObservableCollection<string>(ResultList);

            LBInvites.ItemsSource = Invites;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Запросить у сервера новый инвайт и отобразить его

            string Result;

            Network.SendRequest("http://localhost:8000/createInvite", "POST",
            Encoding.GetEncoding("utf-8").GetBytes(SessionKey), "text/plain", out Result);

            if(!string.IsNullOrEmpty(Result))
            {
                Invites.Add(Result);
            }
        }
    }
}
