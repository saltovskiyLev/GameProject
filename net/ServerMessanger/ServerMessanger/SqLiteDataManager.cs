using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


class SqLiteDataManager : IDataManager
{

    SqliteConnection connection;
    private string sourcString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    Random r = new Random();

    public SqLiteDataManager(string ConnectionString)
    {
        connection = new SqliteConnection(ConnectionString);
        connection.Open();
    }

    public bool Auth(string login, string password)
    {
        SqliteCommand sqliteCommand = PrepareCommand("SELECT * FROM users WHERE login = '" + login + "'");

        bool result;

        using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
        {
            
            if (reader.HasRows)
            {
                reader.Read();
                string pass = (string)(reader.GetValue(2));
                if(pass == password)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }
        }

        return result;
    }



    public bool CheckLoginAvailability(string login)
    {
        string request = string.Format("SELECT * FROM users WHERE login = '{0}'", login);
        SqliteCommand sqliteCommand = PrepareCommand(request);

        using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
        {
            if (reader.HasRows)
            {
                return false;   
            }
            else
            {
                return true;
            }
        }
    }

    public User GetUserByLogIn(string login)
    {
        User user = null;

        string request = string.Format("SELECT * FROM users WHERE login = '{0}'", login);
        SqliteCommand sqliteCommand = PrepareCommand(request);

        using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
        {
            if (reader.HasRows)
            {
                reader.Read();
                long? id = (long?)(reader.GetValue(0));

                user = new User();

                user.Id = (long?)(reader.GetValue(0));

                user.Login = login;

                user.Password = (string)(reader.GetValue(2));
            }
        }

        return user;
    }

    public string CreateInvite(int userId)
    {
        string invite = "";

        for(int i = 0; i < 8; i++)
        {
            invite += sourcString.Substring(r.Next(0, sourcString.Length), 1);
        }

        try
        {
            SqliteCommand sqliteCommand1 = PrepareCommand(string.Format("INSERT INTO invites (user_id, invite)" +
                   " VALUES ('{0}', '{1}')", userId, invite));

            sqliteCommand1.ExecuteNonQuery();
        }
        catch
        {
            invite = "";
        }


        return invite;
    }

    public bool Register(string login, string password, string userName)
    {
        bool success = true;


        try
        {
            
            SqliteCommand sqliteCommand1 = PrepareCommand(string.Format("INSERT INTO users (login, password, name)" +
                " VALUES ('{0}', '{1}', '{2}')", login, password, userName));

            sqliteCommand1.ExecuteNonQuery();
        }

        catch (Exception er)
        {
            Console.WriteLine(er.Message);
            success = false;
        }

        return success;
    }

    private SqliteCommand PrepareCommand(string request)
    {
        SqliteCommand command = new SqliteCommand();
        command.Connection = connection;
        command.CommandText = request;

        return command;
    }

    public List<string> GetInvites(int userId)
    {
        List<string> invites = new List<string>();

        SqliteCommand command = PrepareCommand("SELECT * FROM invites WHERE user_id =" + userId);

        using (SqliteDataReader reader = command.ExecuteReader())
        {
            if (reader.HasRows)
            {
                while(reader.Read())
                {
                    string invite = (string)(reader.GetValue(2));
                    invites.Add(invite);
                }
            }
        }
        return invites;
    }
}
