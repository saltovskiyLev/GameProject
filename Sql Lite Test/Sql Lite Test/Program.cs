using Microsoft.Data.Sqlite;


// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
TestSelect();


void TestInsert()
{
    using (SqliteConnection connection = new SqliteConnection(@"Data Source=E:\\GameProject\\GameProject\\net\\ServerMessanger\\ServerMessanger\\bin\\Debug\\net6.0\\messangerDb.db"))
    {
        connection.Open();
        SqliteCommand command = new SqliteCommand();
        command.Connection = connection;
        command.CommandText = "INSERT INTO users (login, password, name) VALUES ('1', '2', '3'), ('4', '5', '6')";
        int count = command.ExecuteNonQuery();
        Console.WriteLine("Вставленно рядов: " + count);
        connection.Close();
    }
}

void TestSelect()
{
    using (SqliteConnection connection = new SqliteConnection(@"Data Source=E:\\GameProject\\GameProject\\net\\ServerMessanger\\ServerMessanger\\bin\\Debug\\net6.0\\messangerDb.db"))
    {
        connection.Open();
        SqliteCommand command = new SqliteCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM users";

        using(SqliteDataReader reader = command.ExecuteReader())
        {
            if(reader.HasRows)
            {
                reader.Read();
                long? id = (long?)(reader.GetValue(0));


                Console.WriteLine(id.ToString());
            }
        }
        connection.Close();
    }
}

