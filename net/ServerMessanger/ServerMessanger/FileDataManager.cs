using System.IO;

class FileDataManager : IDataManager
{
    List<User> users;

    Random r = new Random();
    public string UserFolderPath = "E:\\GameProject\\GameProject\\net\\ServerMessanger\\ServerMessanger\\users";


    public FileDataManager(string usersFolder = "")
    {
        if(usersFolder != "")
        {
            UserFolderPath = usersFolder;
        }

        users = new List<User>();


        string[] files = Directory.GetFiles(UserFolderPath);
        for (int i = 0; i < files.Length; i++)
        {
            string[] lines = File.ReadAllLines(files[i]);

            User u = new User();
            u.Login = lines[0];
            u.Password = lines[1];
            // TODO: сохранить ID

            users.Add(u);
        }
    }

    public bool CheckLoginAvailability(string login)
    {
        string[] files = Directory.GetFiles(UserFolderPath);
        for (int i = 0; i < files.Length; i++)
        {
            string[] lines = File.ReadAllLines(files[i]);

            if (lines[0] == login)
            {
                return false;
            }
        }
        return true;
    }

    public Int64 GetNewId()
    {
        Int64 id;
        while (true)
        {
            id = r.Next() * r.Next();

            if (!File.Exists(UserFolderPath + "\\" + id + ".txt"))
            {
                break;
            }
        }
        return id;

    }

    public bool Register(string login, string password)
    {
        bool success = true;

        Int64 Id = GetNewId();

        try
        {
            string UserFile = login + "\n" + password;
            File.WriteAllText(UserFolderPath + "\\" + Id + ".txt", UserFile);
        }

        catch(Exception er)
        {
            Console.WriteLine(er.Message);
            success = false;
        }

        return success;
    }

    public bool Auth(string login, string password)
    {
        var user = users.Where(u => u.Login == login).FirstOrDefault();

        if(user == null)
        {
            return false;
        }

        if(user.Password != password)
        {
            return false;
        }

        return true;
    }
}

