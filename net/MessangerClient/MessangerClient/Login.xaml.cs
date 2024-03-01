using MessangerCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MessangerClient
{

    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        public bool LogInResult = false;

        public Login()
        {
            InitializeComponent();
        }

        public string SessionKey = "";

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {

            string login = TBLoG.Text;
            string pass = PBPass.Password;
            string Result;

            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Поле login не должно быть пустым");
                return;
            }

            RequestResult requestResult;
            requestResult = Network.SendRequest("http://localhost:8000/auth", "POST",
            Encoding.GetEncoding("utf-8").GetBytes(login + "*" + pass), "text/plain", out Result);


            if(requestResult == RequestResult.Sucsess)
            {
                MessageBox.Show("Вы успешно вошли в систему");

                SessionKey = Result;
                LogInResult = true;

                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка входа: " + requestResult.ToString() + " " + Result);
            }
        }

        private void Registr_Click(object sender, RoutedEventArgs e)
        {

            RequestResult requestResult;


            LogInResult = false;
            string login, pass;

            login = TBLoG.Text;
            pass = PBPass.Password;

            if(string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Поле login не должно быть пустым");
                return;
            }

            string Result;

            // Проверка того, что login не занят
            requestResult = Network.SendRequest("http://localhost:8000/checklogin", "POST",
                Encoding.GetEncoding("utf-8").GetBytes(login),
                "text/plain",  out Result);

            if(requestResult == RequestResult.TimeOut)
            {
                MessageBox.Show("Сервер не отвечает");
                return;
            }

            else if(requestResult == RequestResult.NotAvailible)
            {
                MessageBox.Show("Такой логин занят");
                return;
            }
            // регистрация нового пользователя
            try
            {
                requestResult = Network.SendRequest("http://localhost:8000/register", "POST",
                Encoding.GetEncoding("utf-8").GetBytes(login + "*" + pass + "*" + UserNameBox.Text), "text/plain", out Result);

                if(requestResult == RequestResult.Sucsess)
                {
                    MessageBox.Show("Вы зарегестрированы");
                }
                else
                {
                    MessageBox.Show("Ошибка регистрации " + requestResult.ToString());
                }

            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }


        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            LogInResult = false;
            Close();
        }
    }
}
