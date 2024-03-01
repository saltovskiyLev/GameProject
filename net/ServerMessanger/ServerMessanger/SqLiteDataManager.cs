using MessangerCore;
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

    //public bool UseInvite(string SessionKey, string Invite)

    public void AddContact(int FirstId, int SecondId)
    {
        SqliteCommand sqliteCommand1 = PrepareCommand(string.Format("INSERT INTO contacts (user1, user2)" +
       " VALUES ('{0}', '{1}')", FirstId, SecondId));

        sqliteCommand1.ExecuteNonQuery();

        SqliteCommand sqliteCommand2 = PrepareCommand(string.Format("INSERT INTO contacts (user1, user2)" +
        " VALUES ('{1}', '{0}')", FirstId, SecondId));

        sqliteCommand2.ExecuteNonQuery();
    }
    

    public List<User> GetFriends(string login)
    {
        List<User> users = new List<User>();

        User user = GetUser(login);

        SqliteCommand sqliteCommand = PrepareCommand("SELECT user2 FROM contacts WHERE user1 = " + user.Id);


        using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
        {

            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    long id = (long)reader.GetValue(0);

                    User u = GetUser((int)id);

                    users.Add(u);
                }
            }

        }

        return users;
    }



    int GetIdByLogin(string login)
    {
        int Id = 0;

        string request = string.Format("SELECT id FROM users WHERE login = '{0}'", login);
        SqliteCommand sqliteCommand = PrepareCommand(request);

        using(SqliteDataReader reader = sqliteCommand.ExecuteReader())
        {
            if(reader.HasRows)
            {
                reader.Read();
                Id = (int)(reader.GetValue(0));
            }
        }
        return Id;
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

    public User GetUser(string login)
    {
        User user = null;

        string request = string.Format("SELECT * FROM users WHERE login = '{0}'", login);

        user = GetUserByQuery(request);

        return user;
    }

    public User GetUser(int id)
    {
        User user = null;

        string request = string.Format("SELECT * FROM users WHERE id = '{0}'", id);

        user = GetUserByQuery(request);


        return user;
    }

    private User GetUserByQuery(string request)
    {
        User user = null;

        SqliteCommand sqliteCommand = PrepareCommand(request);

        using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
        {
            if (reader.HasRows)
            {
                reader.Read();
                long? id = (long?)(reader.GetValue(0));

                user = new User();

                user.Id = (long?)(reader.GetValue(0));

                user.Login = (string)reader.GetValue(1);

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

    public int GetUserIdByInvite(string InviteCode)
    {
        int userId = 0;

        try
        {
            SqliteCommand command = PrepareCommand("SELECT user_id FROM invites WHERE invite = '" + InviteCode + "'");
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    var v = reader.GetValue(0);
                    Type t = v.GetType();

                    userId = (int)(long)(reader.GetValue(0));
                }
            }
        }

        catch
        {

        }

        return userId;
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
