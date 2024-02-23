using System.Net;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using MessangerCore;
using Newtonsoft.Json;

// See https://aka.ms/new-console-template for more information

HttpListener listener = new HttpListener();
listener.Prefixes.Add("http://localhost:8000/");
listener.Prefixes.Add("http://localhost:8000/checklogin/");
listener.Prefixes.Add("http://localhost:8000/register/");
IDataManager dataManager = new SqLiteDataManager(@"Data Source=E:\GameProject\GameProject\net\ServerMessanger\ServerMessanger\messangerDb.db");
ISessionManager sessionManager = new SimpleSessionManager();

listener.Start();
Console.WriteLine("server started");
while (true)
{
    HttpListenerContext context = listener.GetContext();
    HttpListenerRequest request = context.Request;
    //request.InputStream
    Console.WriteLine(request.Url);
    Console.WriteLine(request.UserAgent);
    string requestBody = "";
    using (StreamReader reader = new StreamReader(request.InputStream))
    {
        requestBody = reader.ReadToEnd();
        Console.WriteLine(requestBody);
    }
    HttpListenerResponse response = context.Response;
    /*respones.Headers.Set(HttpResponseHeader.ContentType, "text/html");
    string data = "    <html>\r\n        <head>\r\n            <meta charset='utf8'>\r\n            <title>Hello</title>\r\n        </head>\r\n        <body>\r\n            <h2>Hello World!</h2>\r\n        </body>\r\n    </html>";
    byte[] buffer = Encoding.UTF8.GetBytes(data);
    respones.ContentLength64 = buffer.Length;
    respones.OutputStream.Write(buffer, 0, buffer.Length);*/

    string[] url = ParseURL(request.Url.AbsoluteUri);
    
    switch (url[2])
    {
        case "checklogin":
            CheckLoginAvailability(requestBody, response);
            break;

        case "register":
            {
                /*string login, password;
                int splitter = requestBody.IndexOf('*');
                login = requestBody.Substring(0, splitter);
                password = requestBody.Substring(splitter + 1);*/

                string[] userData = requestBody.Split('*');

                Register(userData[0], userData[1], userData[2], response);
            }
                break;
            
        case "auth":
            {
                string login, password;
                int splitter = requestBody.IndexOf('*');
                login = requestBody.Substring(0, splitter);
                password = requestBody.Substring(splitter + 1);
                if(dataManager.Auth(login, password))
                {
                    string key = sessionManager.CreateSession(login);
                    SendTextResponse(response, key, 200);
                }
                else
                {
                    SendTextResponse(response, "", 403);
                }
            }
            break;

        case "createInvite":
            {
                string sessionKey = requestBody;
                string login = sessionManager.GetLogin(sessionKey);

                if (string.IsNullOrEmpty(login)) 
                {
                    SendTextResponse(response, "Вы не вошли в систему", 403);
                }

                else
                {
                    User user = dataManager.GetUserByLogIn(login);
                    string invite = dataManager.CreateInvite((int)(user.Id));

                    if(invite == "")
                    { 
                        SendTextResponse(response, "Ошибка генерации приглашения", 500);
                    }
                    else
                    {
                        SendTextResponse(response, invite, 200);
                    }
                }
                break;
            }

        case "UseInvite":
            {
                {
                    List<string> par = JsonConvert.DeserializeObject<List<string>>(requestBody);
                    string login = sessionManager.GetLogin(par[0]);

                    if (string.IsNullOrEmpty(login))
                    {
                        SendTextResponse(response, "Вы не вошли в систему", 403);
                    }

                    else
                    {
                        User user = dataManager.GetUserByLogIn(login);

                        int id2 = dataManager.GetUserIdByInvite(par[1]);

                        dataManager.AddContact((int)(user.Id), id2);
                    }
                    break;
                }
            }
                
        case "GetInvites":
            {
                string sessionKey = requestBody;
                string login = sessionManager.GetLogin(sessionKey);

                if (string.IsNullOrEmpty(login))
                {
                    SendTextResponse(response, "Вы не вошли в систему", 403);
                }

                else
                {
                    User user = dataManager.GetUserByLogIn(login);
                    List<string> Invites = dataManager.GetInvites((int)user.Id);

                    SendTextResponse(response, JsonConvert.SerializeObject(Invites), 200);


                } 
                break;
            }
    }

}

string[] ParseURL(string URL)
{
    return URL.Split('/', StringSplitOptions.RemoveEmptyEntries);
}

void SendTextResponse(HttpListenerResponse response, string data, int status)
{
    response.Headers.Set(HttpResponseHeader.ContentType, "text/plain");
    response.StatusCode = status;
    byte[] buffer = Encoding.UTF8.GetBytes(data);
    response.ContentLength64 = buffer.Length;
    response.OutputStream.Write(buffer, 0, buffer.Length);
}

void CheckLoginAvailability(string login, HttpListenerResponse response)
{
    bool result = dataManager.CheckLoginAvailability(login);

    /*response.Headers.Set(HttpResponseHeader.ContentType, "text/plain");
    string data = result.ToString();
    byte[] buffer = Encoding.UTF8.GetBytes(data);
    response.ContentLength64 = buffer.Length;
    response.OutputStream.Write(buffer, 0, buffer.Length);*/
    SendTextResponse(response, result.ToString(), 200);
}

void Register(string login, string password, string userName, HttpListenerResponse response)
{
    bool result = false;

    if (dataManager.CheckLoginAvailability(login))
    {
         result = dataManager.Register(login, password, userName);
    }

    SendTextResponse(response, result.ToString(), result ? 200 : 500);
}

