public class UserService
{
    private Database database;

    public UserService(Database db)
    {
        database = db;
    }

    public void RegisterUser(User user)
    {
        database.AddUser(user);
    }

    public bool AuthenticateUser(string username, string password)
    {
        return database.AuthenticateUser(username, password);
    }
}