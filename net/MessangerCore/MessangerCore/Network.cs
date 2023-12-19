using System.Net;

namespace MessangerCore
{
    public class Network
    {
        static public RequestResult SendRequest(string adress, string type, byte[] data, string contentType, out string resultString)
        {
            RequestResult result;

            resultString = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(adress);
                request.Method = type;
                request.Timeout = 30000;
                if (type == "POST")
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
                    if (response.StatusCode == HttpStatusCode.NotFound)
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
                //MessageBox.Show(e.Message);
                result = RequestResult.NotAvailible;
            }

            return result;

        }
    }
}