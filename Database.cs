using System.Data.SQLite;

public class Database
{
    private SQLiteConnection connection;

    public Database(string databasePath)
    {
        connection = new SQLiteConnection($"Data Source={databasePath};Version=3;");
        connection.Open();
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT UNIQUE,
                    Password TEXT
                );
                CREATE TABLE IF NOT EXISTS Messages (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Sender TEXT,
                    Receiver TEXT,
                    Content TEXT,
                    Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP
                );";
            command.ExecuteNonQuery();
        }
    }

    public void AddUser(User user)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Password", user.Password); // В реальных приложениях пароли должны быть хэшированы
            command.ExecuteNonQuery();
        }
    }

    public bool AuthenticateUser(string username, string password)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);
            long count = (long)command.ExecuteScalar();
            return count > 0;
        }
    }

    public void AddMessage(string sender, string receiver, string content)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "INSERT INTO Messages (Sender, Receiver, Content) VALUES (@Sender, @Receiver, @Content)";
            command.Parameters.AddWithValue("@Sender", sender);
            command.Parameters.AddWithValue("@Receiver", receiver);
            command.Parameters.AddWithValue("@Content", content);
            command.ExecuteNonQuery();
        }
    }

    public List<string> GetMessages(string sender, string receiver)
    {
        var messages = new List<string>();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT Content FROM Messages WHERE Sender = @Sender AND Receiver = @Receiver";
            command.Parameters.AddWithValue("@Sender", sender);
            command.Parameters.AddWithValue("@Receiver", receiver);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    messages.Add(reader.GetString(0));
                }
            }
        }
        return messages;
    }
}