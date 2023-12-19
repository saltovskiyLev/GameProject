using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Newtonsoft.Json;

namespace MessangerClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string sessionKey = "";
        public MainWindow()
        {
            InitializeComponent();

            ObservableCollection<Contact> contacts = new ObservableCollection<Contact>();

            contacts.Add(new Contact { Name = "Lev" });
            contacts.Add(new Contact { Name = "Alex" });
            contacts.Add(new Contact { Name = "Max" });


            LBContacts.ItemsSource = contacts;

            Login login = new Login();
            login.ShowDialog();
            sessionKey = login.SessionKey;

            if (login.LogInResult == false)
            {
                Close();
            }
        }

        private void Invite_Click(object sender, RoutedEventArgs e)
        {
            invitesWindow invites = new invitesWindow(sessionKey);

            invites.ShowDialog();
        }
    }

    
}
