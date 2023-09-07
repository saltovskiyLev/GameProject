using System.Net;
using System.Runtime.InteropServices;
using System.Text;

// See https://aka.ms/new-console-template for more information

HttpListener listener = new HttpListener();
listener.Prefixes.Add("http://localhost:8000/");
listener.Prefixes.Add("http://localhost:8000/test");

listener.Start();
Console.WriteLine("server started");
while (true)
{
    HttpListenerContext context = listener.GetContext();
    HttpListenerRequest request = context.Request;
    Console.WriteLine(request.Url);
    Console.WriteLine(request.UserAgent);
    HttpListenerResponse respones = context.Response;
    respones.Headers.Set(HttpResponseHeader.ContentType, "text/html");
    string data = "    <html>\r\n        <head>\r\n            <meta charset='utf8'>\r\n            <title>Hello</title>\r\n        </head>\r\n        <body>\r\n            <h2>Hello World!</h2>\r\n        </body>\r\n    </html>";
    byte[] buffer = Encoding.UTF8.GetBytes(data);
    respones.ContentLength64 = buffer.Length;
    respones.OutputStream.Write(buffer, 0, buffer.Length);

}