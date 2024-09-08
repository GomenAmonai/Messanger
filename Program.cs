using System;

class Program
{
    static void Main()
    {
        var db = new Database("messenger.db");
        var userService = new UserService(db);
        var networkService = new NetworkService();

        Console.WriteLine("Welcome to the Messenger!");

        string? username = null; // Инициализация переменной

        while (true)
        {
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Send Message");
            Console.WriteLine("4. Receive Messages");
            Console.WriteLine("5. Exit");
            var choice = Console.ReadLine();

            string? password;
            switch (choice)
            {
                case "1":
                    Console.Write("Enter username: ");
                    username = Console.ReadLine();
                    Console.Write("Enter password: ");
                    password = Console.ReadLine();
                    if (username != null && password != null)
                    {
                        userService.RegisterUser(new User { Username = username, Password = password });
                        Console.WriteLine("User registered!");
                    }
                    else
                    {
                        Console.WriteLine("Username or password cannot be null.");
                    }
                    break;

                case "2":
                    Console.Write("Enter username: ");
                    username = Console.ReadLine();
                    Console.Write("Enter password: ");
                    password = Console.ReadLine();
                    if (username != null && password != null)
                    {
                        if (userService.AuthenticateUser(username, password))
                        {
                            Console.WriteLine("Login successful!");
                            networkService.ConnectToServer("localhost", 8888);
                        }
                        else
                        {
                            Console.WriteLine("Invalid credentials.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Username or password cannot be null.");
                    }
                    break;

                case "3":
                    if (username == null)
                    {
                        Console.WriteLine("You need to login first.");
                        break;
                    }
                    Console.Write("Enter receiver: ");
                    var receiver = Console.ReadLine();
                    Console.Write("Enter message: ");
                    var message = Console.ReadLine();
                    if (receiver != null && message != null)
                    {
                        networkService.SendMessage(message);
                        db.AddMessage(username, receiver, message);
                        Console.WriteLine("Message sent!");
                    }
                    else
                    {
                        Console.WriteLine("Receiver or message cannot be null.");
                    }
                    break;

                case "4":
                    Console.Write("Enter sender: ");
                    var sender = Console.ReadLine();
                    Console.Write("Enter receiver: ");
                    receiver = Console.ReadLine();
                    if (sender != null && receiver != null)
                    {
                        var messages = db.GetMessages(sender, receiver);
                        Console.WriteLine("Messages:");
                        foreach (var msg in messages)
                        {
                            Console.WriteLine(msg);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Sender or receiver cannot be null.");
                    }
                    break;

                case "5":
                    return;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}