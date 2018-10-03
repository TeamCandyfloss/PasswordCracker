using System;
using System.IO;
using System.Net.Sockets;

namespace PasswordCrackerClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient clientSocket = new TcpClient("localhost", 6789);

            Stream ns = clientSocket.GetStream();
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true;
            StreamReader sr = new StreamReader(ns);



            for (int i = 0; i <= 4; i++)
            {

                string messageToServer = Console.ReadLine();
                sw.WriteLine(messageToServer);

                string messageFromServer = sr.ReadLine();
                Console.WriteLine("Server: " + messageFromServer);
            }


            Console.ReadLine();




            ns.Close();
            clientSocket.Close();
        }
    }
}
