using System.Net;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;

// See https://aka.ms/new-console-template for more information

HttpListener listener = new HttpListener();
listener.Prefixes.Add("http://localhost:8000/");
listener.Prefixes.Add("http://localhost:8000/checklogin/");
listener.Prefixes.Add("http://localhost:8000/register/");
IDataManager dataManager = new FileDataManager();

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
    HttpListenerResponse respones = context.Response;
    /*respones.Headers.Set(HttpResponseHeader.ContentType, "text/html");
    string data = "    <html>\r\n        <head>\r\n            <meta charset='utf8'>\r\n            <title>Hello</title>\r\n        </head>\r\n        <body>\r\n            <h2>Hello World!</h2>\r\n        </body>\r\n    </html>";
    byte[] buffer = Encoding.UTF8.GetBytes(data);
    respones.ContentLength64 = buffer.Length;
    respones.OutputStream.Write(buffer, 0, buffer.Length);*/

    string[] url = ParseURL(request.Url.AbsoluteUri);
    
    switch (url[2])
    {
        case "checklogin":
            CheckLogin(requestBody, respones);
            break;

        case "register":

            string login, password;
            int splitter = requestBody.IndexOf('*');
            login = requestBody.Substring(0, splitter);
            password = requestBody.Substring(splitter + 1);

            Register(login, password, respones);
                break;
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

void CheckLogin(string login, HttpListenerResponse response)
{
    bool result = dataManager.CheckLoginAvailability(login);

    /*response.Headers.Set(HttpResponseHeader.ContentType, "text/plain");
    string data = result.ToString();
    byte[] buffer = Encoding.UTF8.GetBytes(data);
    response.ContentLength64 = buffer.Length;
    response.OutputStream.Write(buffer, 0, buffer.Length);*/
    SendTextResponse(response, result.ToString(), 200);
}

void Register(string login, string password, HttpListenerResponse response)
{
    bool result = false;

    if (dataManager.CheckLoginAvailability(login))
    {
         result = dataManager.Register(login, password);
    }

    SendTextResponse(response, result.ToString(), result ? 200 : 500);
}

