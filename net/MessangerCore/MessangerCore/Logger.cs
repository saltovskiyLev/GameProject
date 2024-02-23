using System.IO;


namespace MessangerCore
{
    public class Logger
    {
        static public void Write(string message)
        {
            File.AppendAllText("Log.txt", DateTime.Now.ToString() + " " + message);

        }
    }
}