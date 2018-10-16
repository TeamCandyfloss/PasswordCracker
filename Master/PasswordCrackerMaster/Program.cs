using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCrackerMaster
{
    class Program
    {
        private static bool _StartTimer;
        static void Main(string[] args)
        {
            
            TcpListener serverSocket = new TcpListener(6789);

            //TcpListener serverSocket = new TcpListener(6789);
            serverSocket.Start();
            Console.WriteLine("Server activated now");
            


            while (true)
            {
                TcpClient connectionSocket = serverSocket.AcceptTcpClient();
                
                Console.WriteLine($"{LogHandler.GetUsers()} connected");
                MasterThreadDelegate service = new MasterThreadDelegate(connectionSocket, 10000);

                Task.Factory.StartNew(() => service.Start());
                if (_StartTimer)
                {
                    LogHandler.StartStopWatch();
                    _StartTimer = false;
                }
               

            }


            
          
           

            
        }
    }
}
