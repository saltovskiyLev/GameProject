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

            RequestResult requestResult;

            requestResult = SendRequest("http://localhost:8000/auth", "POST",
            Encoding.GetEncoding("utf-8").GetBytes(login + "*" + pass), "text/plain", out Result);


            if(requestResult == RequestResult.Sucsess)
            {
                MessageBox.Show("Вы успешно вошли в систему");

                

                SessionKey = Result;
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

            string Result;

            
            requestResult = SendRequest("http://localhost:8000/checklogin", "POST",
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

            try
            {
                requestResult = SendRequest("http://localhost:8000/register", "POST",
                Encoding.GetEncoding("utf-8").GetBytes(login + "*" + pass), "text/plain", out Result);

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

        RequestResult SendRequest(string adress, string type, byte[] data, string contentType, out string resultString)
        {
            RequestResult result;

            resultString = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(adress);
                request.Method = type;
                request.Timeout = 30000;
                if(type == "POST")
                {
                    request.ContentLength = data.Length;
                    request.ContentType = contentType;
                    Stream stream = request.GetRequestStream();
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }
                request.Headers.Add(HttpRequestHeader.UserAgent, "LevClient...");
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if(response.StatusCode == HttpStatusCode.NotFound)
                    {
                        result = RequestResult.TimeOut;
                    }
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            resultString = reader.ReadToEnd();
                            result = RequestResult.Sucsess;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                result = RequestResult.NotAvailible;
            }

            return result;

        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            LogInResult = false;
            Close();
        }
    }
}
