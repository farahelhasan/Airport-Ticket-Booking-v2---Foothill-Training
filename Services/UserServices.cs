using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Services
{
    class UserServicees
    {
       public const string UsersFile= "C:\\Users\\pc\\source\\repos\\Airport Ticket Booking\\Data\\user.csv";
       public static string Login()
        {
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            string userFile = UsersFile;  

            if (File.Exists(userFile))
            {
                var lines = File.ReadAllLines(userFile);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3 && parts[0] == username && parts[1] == password)
                    {
                        Console.WriteLine($"\nLogin successful! Welcome, {username}");
                        return parts[2];  // Return role (Manager/Passenger)
                    }
                }
            }

            Console.WriteLine("Invalid username or password. Try again.");
            return Login();  // Recursively retry login
        }

    }
}
