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
using Newtonsoft.Json;

namespace MessangerClient
{
    /// <summary>
    /// Логика взаимодействия для UserInvite.xaml
    /// </summary>
    public partial class UserInvite : Window
    {

        string SesissionKey;

        public UserInvite(string Sessionkey)
        {
            InitializeComponent();
            SesissionKey = Sessionkey;

        }

        private void Invite_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Invite.Clear();

          
        }

        private void UseInvite_Click(object sender, RoutedEventArgs e)
        {
            string Result;

            List<string> E = new List<string>();

            E.Add(SesissionKey);

            // не совсем понялб, откуда брать sessionKey

            E.Add(Invite.Text);


            string InviteCod = JsonConvert.SerializeObject(E);


            RequestResult requestResult;
            requestResult = Network.SendRequest("http://localhost:8000/UseInvite", "POST",
            Encoding.GetEncoding("utf-8").GetBytes(InviteCod), "text/plain", out Result);
        }
    }
}
