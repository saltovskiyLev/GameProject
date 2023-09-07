using System.Net;

// See https://aka.ms/new-console-template for more information



Console.WriteLine("Hello, World!");

try
{
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:8000/");
    request.Method = "GET";
    request.Headers.Add(HttpRequestHeader.UserAgent, "LevClient...");
    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
    {
        using (Stream stream = response.GetResponseStream())
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                Console.WriteLine(reader.ReadToEnd());
            }
        }
    }
}
catch(Exception e)
{
    Console.WriteLine(e.Message);
}
