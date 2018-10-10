using System;
using System.IO;
using System.Net.Sockets;

namespace PasswordCrackerClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();

            client.Create("localhost", 6789);

            Console.ReadLine();
        }
    }
}
